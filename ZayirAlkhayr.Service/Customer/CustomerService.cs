using System;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Interface.Customer;
using ZayirAlkhayr.Interface.Repositories;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service.Customer
{
    public class CustomerService: ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponseModel<string>> AddNewCustomer(ZayirAlkhayr.Entities.Models.Customer Model)
        {
            try
            {
                Model.InsertDate = DateTime.UtcNow;
                await _unitOfWork.Repository<ZayirAlkhayr.Entities.Models.Customer>().AddAsync(Model);
                await _unitOfWork.CompleteAsync();
                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception ex)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
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
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }
    }
}
