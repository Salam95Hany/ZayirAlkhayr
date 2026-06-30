using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models.EmployeeModel;

namespace ZayirAlkhayr.Interface.Employee
{
    public interface IEmployeeSalaryService
    {
        Task<ApiResponseModel<List<Salary>>> GetEmployeeSalary(DateTime SalaryDate);
        Task<ApiResponseModel<bool>> CheckMonthEmployeePaid(DateTime SalaryDate);
        Task<ApiResponseModel<string>> CreateMonthEmployeeSalary(DateTime SalaryDate, string InsertUser);
        Task<ApiResponseModel<string>> PaidEmployeeSalary(List<Salary> Model, DateTime SalaryDate, string UpdateUser);
        Task<ApiResponseModel<string>> PayrollTransferToExpenses(double TotalAmount, DateTime SalaryDate, string InsertUser);
    }
}
