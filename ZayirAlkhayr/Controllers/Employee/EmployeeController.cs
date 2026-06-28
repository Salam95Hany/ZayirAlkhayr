using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeSalaryService _employeeSalaryService;
        public EmployeeController(IEmployeeService employeeService, IEmployeeSalaryService employeeSalaryService)
        {
            _employeeService = employeeService;
            _employeeSalaryService = employeeSalaryService;
        }

        [HttpPost("GetAllEmployees")]
        public async Task<ApiResponseModel<DataTable>> GetAllEmployees(PagingFilterModel PagingFilter)
        {
            var results = await _employeeService.GetAllEmployees(PagingFilter);
            return results;
        }

        [HttpPost("GetAllEmployeeFilters")]
        public async Task<ApiResponseModel<List<FilterModel>>> GetAllEmployeeFilters(PagingFilterModel PagingFilter)
        {
            var results = await _employeeService.GetAllEmployeeFilters(PagingFilter);
            return results;
        }

        [HttpGet("GetAllJobTitle")]
        public async Task<ApiResponseModel<List<JobTitle>>> GetAllJobTitle()
        {
            var results = await _employeeService.GetAllJobTitle();
            return results;
        }

        [HttpPost("AddNewJobTitle")]
        public async Task<ApiResponseModel<string>> AddNewJobTitle(JobTitle Model)
        {
            var results = await _employeeService.AddNewJobTitle(Model);
            return results;
        }

        [HttpPost("AddNewEmployee")]
        public async Task<ApiResponseModel<string>> AddNewEmployee(ZayirAlkhayr.Entities.Models.EmployeeModel.Employee Model)
        {
            var results = await _employeeService.AddNewEmployee(Model);
            return results;
        }

        [HttpPost("UpdateJobTitle")]
        public async Task<ApiResponseModel<string>> UpdateJobTitle(JobTitle Model)
        {
            var results = await _employeeService.UpdateJobTitle(Model);
            return results;
        }

        [HttpPost("UpdateEmployee")]
        public async Task<ApiResponseModel<string>> UpdateEmployee(ZayirAlkhayr.Entities.Models.EmployeeModel.Employee Model)
        {
            var results = await _employeeService.UpdateEmployee(Model);
            return results;
        }

        [HttpGet("DeleteJobTitle")]
        public async Task<ApiResponseModel<string>> DeleteJobTitle(int JobTitleId)
        {
            var results = await _employeeService.DeleteJobTitle(JobTitleId);
            return results;
        }

        [HttpGet("DeleteEmployee")]
        public async Task<ApiResponseModel<string>> DeleteEmployee(int EmployeeId)
        {
            var results = await _employeeService.DeleteEmployee(EmployeeId);
            return results;
        }

        [HttpGet("GetEmployeeSalary")]
        public async Task<ApiResponseModel<List<Salary>>> GetEmployeeSalary(DateTime SalaryDate)
        {
            var results = await _employeeSalaryService.GetEmployeeSalary(SalaryDate);
            return results;
        }

        [HttpGet("CheckMonthEmployeePaid")]
        public async Task<ApiResponseModel<bool>> CheckMonthEmployeePaid(DateTime SalaryDate)
        {
            var results = await _employeeSalaryService.CheckMonthEmployeePaid(SalaryDate);
            return results;
        }

        [HttpGet("CreateMonthEmployeeSalary")]
        public async Task<ApiResponseModel<string>> CreateMonthEmployeeSalary(DateTime SalaryDate, string InsertUser)
        {
            var results = await _employeeSalaryService.CreateMonthEmployeeSalary(SalaryDate, InsertUser);
            return results;
        }

        [HttpPost("PaidEmployeeSalary")]
        public async Task<ApiResponseModel<string>> PaidEmployeeSalary(List<Salary> Model, DateTime SalaryDate, string UpdateUser)
        {
            var results = await _employeeSalaryService.PaidEmployeeSalary(Model, SalaryDate, UpdateUser);
            return results;
        }

        [HttpGet("PayrollTransferToExpenses")]
        public async Task<ApiResponseModel<string>> PayrollTransferToExpenses(double TotalAmount, DateTime SalaryDate, string Reason, string InsertUser)
        {
            var results = await _employeeSalaryService.PayrollTransferToExpenses(TotalAmount, SalaryDate, Reason, InsertUser);
            return results;
        }
    }
}
