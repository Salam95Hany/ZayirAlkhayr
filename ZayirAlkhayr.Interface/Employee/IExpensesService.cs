using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models.EmployeeModel;

namespace ZayirAlkhayr.Interface.Employee
{
    public interface IExpensesService
    {
        Task<ApiResponseModel<DataTable>> GetAllExpenses(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<List<FilterModel>>> GetAllExpensesFilters(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<List<ExpenseCategory>>> GetAllExpenseCategory();
        Task<ApiResponseModel<string>> AddNewExpenseCategory(ExpenseCategory Model);
        Task<ApiResponseModel<string>> AddNewExpenses(Expenses Model);
        Task<ApiResponseModel<string>> UpdateExpenseCategory(ExpenseCategory Model);
        Task<ApiResponseModel<string>> UpdateExpenses(Expenses Model);
        Task<ApiResponseModel<string>> DeleteExpenseCategory(int ExpenseCategoryId);
        Task<ApiResponseModel<string>> DeleteExpenses(int ExpensesId);
    }
}
