using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models.EmployeeModel;
using ZayirAlkhayr.Interface.Common;
using ZayirAlkhayr.Interface.Employee;
using ZayirAlkhayr.Interface.Repositories;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service.Employee
{
    public class EmployeeService: IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISQLHelper _sQLHelper;
        public EmployeeService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper)
        {
            _unitOfWork = unitOfWork;
            _sQLHelper = sQLHelper;
        }

        public async Task<ApiResponseModel<DataTable>> GetAllEmployees(PagingFilterModel PagingFilter)
        {
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[1] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[2] = new SqlParameter("@IsFilter", false);
            Params[3] = new SqlParameter("@FilterList", FilterDt);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[POS].[SP_GetAllEmployees]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<List<FilterModel>>> GetAllEmployeeFilters(PagingFilterModel PagingFilter)
        {
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[1] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[2] = new SqlParameter("@IsFilter", true);
            Params[3] = new SqlParameter("@FilterList", FilterDt);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[POS].[SP_GetAllEmployees]", Params);
            var Filters = dt.ToGroupedFilters();
            return ApiResponseModel<List<FilterModel>>.Success(GenericErrors.GetSuccess, Filters);
        }

        public async Task<ApiResponseModel<List<JobTitle>>> GetAllJobTitle()
        {
            var results = await _unitOfWork.Repository<JobTitle>().GetAllAsync(i => i.IsActive);
            return ApiResponseModel<List<JobTitle>>.Success(GenericErrors.GetSuccess, results);
        }

        public async Task<ApiResponseModel<string>> AddNewJobTitle(JobTitle Model)
        {
            try
            {
                var CheckExist = await _unitOfWork.Repository<JobTitle>().AnyAsync(i => i.Name == Model.Name && i.IsActive);
                if(CheckExist)
                    return ApiResponseModel<string>.Failure(GenericErrors.AlreadyExists);

                Model.IsActive = true;
                Model.InsertDate = DateTime.UtcNow.ToQatarTime();
                await _unitOfWork.Repository<JobTitle>().AddAsync(Model);
                await _unitOfWork.CompleteAsync();
                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception ex)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> AddNewEmployee(ZayirAlkhayr.Entities.Models.EmployeeModel.Employee Model)
        {
            try
            {
                var CheckExist = await _unitOfWork.Repository<JobTitle>().AnyAsync(i => i.Name == Model.FullName && i.IsActive);
                if (CheckExist)
                    return ApiResponseModel<string>.Failure(GenericErrors.AlreadyExists);

                Model.IsActive = true;
                Model.InsertDate = DateTime.UtcNow.ToQatarTime();
                await _unitOfWork.Repository<ZayirAlkhayr.Entities.Models.EmployeeModel.Employee>().AddAsync(Model);
                await _unitOfWork.CompleteAsync();
                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception ex)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> UpdateJobTitle(JobTitle Model)
        {
            try
            {
                var CheckExist = await _unitOfWork.Repository<JobTitle>().AnyAsync(i => i.Name == Model.Name && i.IsActive && i.JobTitleId != Model.JobTitleId);
                if (CheckExist)
                    return ApiResponseModel<string>.Failure(GenericErrors.AlreadyExists);

                var Entity = await _unitOfWork.Repository<JobTitle>().GetByIdAsync(Model.JobTitleId);
                if (Entity != null)
                {
                    Entity.Name = Model.Name;
                    Entity.Description = Model.Description;
                    Entity.UpdateUser = Model.InsertUser;
                    Entity.UpdateDate = DateTime.UtcNow.ToQatarTime();
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

        public async Task<ApiResponseModel<string>> UpdateEmployee(ZayirAlkhayr.Entities.Models.EmployeeModel.Employee Model)
        {
            try
            {
                var CheckExist = await _unitOfWork.Repository<ZayirAlkhayr.Entities.Models.EmployeeModel.Employee>().AnyAsync(i => i.FullName == Model.FullName && i.IsActive && i.EmployeeId != Model.EmployeeId);
                if (CheckExist)
                    return ApiResponseModel<string>.Failure(GenericErrors.AlreadyExists);

                var Entity = await _unitOfWork.Repository<ZayirAlkhayr.Entities.Models.EmployeeModel.Employee>().GetByIdAsync(Model.EmployeeId);
                if (Entity != null)
                {
                    Entity.JobTitleId = Model.JobTitleId;
                    Entity.FullName = Model.FullName;
                    Entity.PhoneNumber = Model.PhoneNumber;
                    Entity.BasicSalary = Model.BasicSalary;
                    Entity.HireDate = Model.HireDate;
                    Entity.Notes = Model.Notes;
                    Entity.UpdateUser = Model.InsertUser;
                    Entity.UpdateDate = DateTime.UtcNow.ToQatarTime();
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

        public async Task<ApiResponseModel<string>> DeleteJobTitle(int JobTitleId)
        {
            try
            {
                var Entity = await _unitOfWork.Repository<JobTitle>().GetByIdAsync(JobTitleId);
                if (Entity != null)
                {
                    Entity.IsActive = false;
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

        public async Task<ApiResponseModel<string>> DeleteEmployee(int EmployeeId)
        {
            try
            {
                var Entity = await _unitOfWork.Repository<ZayirAlkhayr.Entities.Models.EmployeeModel.Employee>().GetByIdAsync(EmployeeId);
                if (Entity != null)
                {
                    Entity.IsActive = false;
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
