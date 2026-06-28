using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models.EmployeeModel;
using ZayirAlkhayr.Interface.Employee;

namespace ZayirAlkhayr.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpensesService _expensesService;
        public ExpensesController(IExpensesService expensesService)
        {
            _expensesService = expensesService;
        }

        [HttpPost("GetAllExpenses")]
        public async Task<ApiResponseModel<DataTable>> GetAllExpenses(PagingFilterModel PagingFilter)
        {
            var results = await _expensesService.GetAllExpenses(PagingFilter);
            return results;
        }

        [HttpPost("GetAllExpensesFilters")]
        public async Task<ApiResponseModel<List<FilterModel>>> GetAllExpensesFilters(PagingFilterModel PagingFilter)
        {
            var results = await _expensesService.GetAllExpensesFilters(PagingFilter);
            return results;
        }

        [HttpGet("GetAllExpenseCategory")]
        public async Task<ApiResponseModel<List<ExpenseCategory>>> GetAllExpenseCategory()
        {
            var results = await _expensesService.GetAllExpenseCategory();
            return results;
        }

        [HttpPost("AddNewExpenseCategory")]
        public async Task<ApiResponseModel<string>> AddNewExpenseCategory(ExpenseCategory Model)
        {
            var results = await _expensesService.AddNewExpenseCategory(Model);
            return results;
        }

        [HttpPost("AddNewExpenses")]
        public async Task<ApiResponseModel<string>> AddNewExpenses(Expenses Model)
        {
            var results = await _expensesService.AddNewExpenses(Model);
            return results;
        }

        [HttpPost("UpdateExpenseCategory")]
        public async Task<ApiResponseModel<string>> UpdateExpenseCategory(ExpenseCategory Model)
        {
            var results = await _expensesService.UpdateExpenseCategory(Model);
            return results;
        }

        [HttpPost("UpdateExpenses")]
        public async Task<ApiResponseModel<string>> UpdateExpenses(Expenses Model)
        {
            var results = await _expensesService.UpdateExpenses(Model);
            return results;
        }

        [HttpGet("DeleteExpenseCategory")]
        public async Task<ApiResponseModel<string>> DeleteExpenseCategory(int ExpenseCategoryId)
        {
            var results = await _expensesService.DeleteExpenseCategory(ExpenseCategoryId);
            return results;
        }

        [HttpGet("DeleteExpenses")]
        public async Task<ApiResponseModel<string>> DeleteExpenses(int ExpensesId)
        {
            var results = await _expensesService.DeleteExpenses(ExpensesId);
            return results;
        }
    }
}
