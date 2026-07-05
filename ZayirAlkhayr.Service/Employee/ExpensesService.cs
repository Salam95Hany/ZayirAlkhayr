using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models.EmployeeModel;
using ZayirAlkhayr.Interface.Common;
using ZayirAlkhayr.Interface.Employee;
using ZayirAlkhayr.Interface.Repositories;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service.Employee
{
    public class ExpensesService : IExpensesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISQLHelper _sQLHelper;
        public ExpensesService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper)
        {
            _unitOfWork = unitOfWork;
            _sQLHelper = sQLHelper;
        }

        public async Task<ApiResponseModel<DataTable>> GetAllExpenses(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[6];
            Params[0] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[1] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[2] = new SqlParameter("@IsFilter", false);
            Params[3] = new SqlParameter("@FilterList", FilterDt);
            Params[4] = new SqlParameter("@FromDate", FromDate);
            Params[5] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Emp].[SP_GetAllExpenses]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<List<FilterModel>>> GetAllExpensesFilters(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[6];
            Params[0] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[1] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[2] = new SqlParameter("@IsFilter", true);
            Params[3] = new SqlParameter("@FilterList", FilterDt);
            Params[4] = new SqlParameter("@FromDate", FromDate);
            Params[5] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Emp].[SP_GetAllExpenses]", Params);
            var Filters = dt.ToGroupedFilters();
            return ApiResponseModel<List<FilterModel>>.Success(GenericErrors.GetSuccess, Filters);
        }

        public async Task<ApiResponseModel<List<ExpenseCategory>>> GetAllExpenseCategory()
        {
            var results = await _unitOfWork.Repository<ExpenseCategory>().GetAllAsync(i => i.IsActive);
            return ApiResponseModel<List<ExpenseCategory>>.Success(GenericErrors.GetSuccess, results);
        }

        public async Task<ApiResponseModel<string>> AddNewExpenseCategory(ExpenseCategory Model)
        {
            try
            {
                var CheckExist = await _unitOfWork.Repository<ExpenseCategory>().AnyAsync(i => i.Name == Model.Name && i.IsActive);
                if (CheckExist)
                    return ApiResponseModel<string>.Failure(GenericErrors.AlreadyExists);

                Model.IsActive = true;
                Model.InsertDate = DateTime.UtcNow.ToQatarTime();
                await _unitOfWork.Repository<ExpenseCategory>().AddAsync(Model);
                await _unitOfWork.CompleteAsync();
                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception ex)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> AddNewExpenses(Expenses Model)
        {
            try
            {
                var CheckExist = await _unitOfWork.Repository<Expenses>().AnyAsync(i => i.ExpenseDate.Year == DateTime.Now.Year && i.ExpenseDate.Month == DateTime.Now.Month
                && i.IsActive && i.ExpenseCategoryId == Model.ExpenseCategoryId);
                if (CheckExist)
                    return ApiResponseModel<string>.Failure(GenericErrors.AlreadyExists);

                Model.IsActive = true;
                Model.InsertDate = DateTime.UtcNow.ToQatarTime();
                await _unitOfWork.Repository<Expenses>().AddAsync(Model);
                await _unitOfWork.CompleteAsync();
                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception ex)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> UpdateExpenseCategory(ExpenseCategory Model)
        {
            try
            {
                var CheckExist = await _unitOfWork.Repository<ExpenseCategory>().AnyAsync(i => i.Name == Model.Name && i.IsActive && i.ExpenseCategoryId != Model.ExpenseCategoryId);
                if (CheckExist)
                    return ApiResponseModel<string>.Failure(GenericErrors.AlreadyExists);

                var Entity = await _unitOfWork.Repository<ExpenseCategory>().GetByIdAsync(Model.ExpenseCategoryId);
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

        public async Task<ApiResponseModel<string>> UpdateExpenses(Expenses Model)
        {
            try
            {
                var Now = DateTime.UtcNow.ToQatarTime();

                var CheckExist = await _unitOfWork.Repository<Expenses>().AnyAsync(x => x.ExpensesId != Model.ExpensesId && x.ExpenseCategoryId == Model.ExpenseCategoryId && x.IsActive &&
                    x.ExpenseDate.Year == Now.Year &&
                    x.ExpenseDate.Month == Now.Month);
                if (CheckExist)
                    return ApiResponseModel<string>.Failure(GenericErrors.AlreadyExists);

                var Entity = await _unitOfWork.Repository<Expenses>().GetByIdAsync(Model.ExpensesId);
                if (Entity != null)
                {
                    Entity.ExpenseCategoryId = Model.ExpenseCategoryId;
                    Entity.Amount = Model.Amount;
                    Entity.Reason = Model.Reason;
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

        public async Task<ApiResponseModel<string>> DeleteExpenseCategory(int ExpenseCategoryId)
        {
            try
            {
                var Entity = await _unitOfWork.Repository<ExpenseCategory>().GetByIdAsync(ExpenseCategoryId);
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

        public async Task<ApiResponseModel<string>> DeleteExpenses(int ExpensesId)
        {
            try
            {
                var Entity = await _unitOfWork.Repository<Expenses>().GetByIdAsync(ExpensesId);
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
