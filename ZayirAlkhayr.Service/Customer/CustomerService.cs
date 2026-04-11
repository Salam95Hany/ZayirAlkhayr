using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Specifications.Customers;
using ZayirAlkhayr.Interface.Common;
using ZayirAlkhayr.Interface.Customer;
using ZayirAlkhayr.Interface.Repositories;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISQLHelper _sQLHelper;
        public CustomerService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper)
        {
            _unitOfWork = unitOfWork;
            _sQLHelper = sQLHelper;
        }

        public async Task<ApiResponseModel<DataTable>> GetAllCustomers(PagingFilterModel PagingFilter)
        {
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[1];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            var dt = await _sQLHelper.ExecuteDataTableAsync("Cust.SP_GetAllCustomersData", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<List<ZayirAlkhayr.Entities.Models.Customer>>> GetCustomerByPhone(string PhoneNumber)
        {
            var Spec = new CustomerSearchSpecification(PhoneNumber);
            var Results = await _unitOfWork.Repository<ZayirAlkhayr.Entities.Models.Customer>().GetAllWithSpecAsync(Spec);
            return ApiResponseModel<List<ZayirAlkhayr.Entities.Models.Customer>>.Success(GenericErrors.GetSuccess, Results);
        }

        public async Task<ApiResponseModel<ZayirAlkhayr.Entities.Models.Customer>> GetCustomerById(int CustomerId)
        {
            var Spec = new CustomerByIdSpecification(CustomerId);
            var Results = await _unitOfWork.Repository<ZayirAlkhayr.Entities.Models.Customer>().GetByIdWithSpecAsync(Spec);
            return ApiResponseModel<ZayirAlkhayr.Entities.Models.Customer>.Success(GenericErrors.GetSuccess, Results);
        }

        public async Task<ApiResponseModel<int>> AddNewCustomer(ZayirAlkhayr.Entities.Models.Customer Model)
        {
            try
            {
                Model.InsertDate = DateTime.UtcNow;
                await _unitOfWork.Repository<ZayirAlkhayr.Entities.Models.Customer>().AddAsync(Model);
                await _unitOfWork.CompleteAsync();
                return ApiResponseModel<int>.Success(GenericErrors.AddSuccess, Model.CustomerId);
            }
            catch (Exception ex)
            {
                return ApiResponseModel<int>.Failure(GenericErrors.TransFailed);
            }

        }

        public async Task<ApiResponseModel<string>> UpdateCustomer(ZayirAlkhayr.Entities.Models.Customer Model)
        {
            try
            {
                var Entity = await _unitOfWork.Repository<ZayirAlkhayr.Entities.Models.Customer>().GetByIdAsync(Model.CustomerId);
                if (Entity != null)
                {
                    Entity.FullName = Model.FullName;
                    Entity.Phone = Model.Phone;
                    Entity.Address = Model.Address;
                    Entity.UpdateUser = Model.InsertUser;
                    Entity.UpdateDate = DateTime.UtcNow;

                    await _unitOfWork.CompleteAsync();
                    return ApiResponseModel<string>.Success(GenericErrors.UpdateSuccess);
                }

                return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

            }
            catch (Exception ex)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

        }

        public async Task<ApiResponseModel<string>> DeleteCustomer(int CustomerId)
        {
            try
            {
                var Entity = await _unitOfWork.Repository<ZayirAlkhayr.Entities.Models.Customer>().GetByIdAsync(CustomerId);
                if (Entity != null)
                {
                    _unitOfWork.Repository<ZayirAlkhayr.Entities.Models.Customer>().Delete(Entity);
                    await _unitOfWork.CompleteAsync();
                    return ApiResponseModel<string>.Success(GenericErrors.DeleteSuccess);
                }

                return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqlException sqlEx)
                {
                    if (sqlEx.Message.Contains("REFERENCE constraint"))
                    {
                        return ApiResponseModel<string>.Failure(GenericErrors.DeleteRelationRow);
                    }
                }

                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }
    }
}
