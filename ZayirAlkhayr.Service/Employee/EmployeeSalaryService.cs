using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models.EmployeeModel;
using ZayirAlkhayr.Interface.Employee;
using ZayirAlkhayr.Interface.Repositories;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service.Employee
{
    public class EmployeeSalaryService : IEmployeeSalaryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeSalaryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponseModel<List<Salary>>> GetEmployeeSalary(DateTime SalaryDate)
        {
            var Salaries = await _unitOfWork.Repository<Salary>().GetAllAsync(i => i.SalaryYear == SalaryDate.Year && i.SalaryMonth == SalaryDate.Month);
            return ApiResponseModel<List<Salary>>.Success(GenericErrors.GetSuccess, Salaries);
        }

        public async Task<ApiResponseModel<bool>> CheckMonthEmployeePaid(DateTime SalaryDate)
        {
            var CheckPaid = await _unitOfWork.Repository<Salary>().AnyAsync(i => i.SalaryYear == SalaryDate.Year && i.SalaryMonth == SalaryDate.Month);
            return ApiResponseModel<bool>.Success(GenericErrors.GetSuccess, CheckPaid);
        }

        public async Task<ApiResponseModel<string>> CreateMonthEmployeeSalary(DateTime SalaryDate, string InsertUser)
        {
            try
            {
                var Employees = await _unitOfWork.Repository<ZayirAlkhayr.Entities.Models.EmployeeModel.Employee>().GetAllAsync(i => i.IsActive);
                if (Employees.Count > 0)
                {
                    var Data = Employees.Select(i => new Salary
                    {
                        EmployeeId = i.EmployeeId,
                        EmployeeName = i.FullName,
                        SalaryYear = SalaryDate.Year,
                        SalaryMonth = SalaryDate.Month,
                        BasicSalary = i.BasicSalary,
                        Bonus = 0,
                        Deduction = 0,
                        Advance = 0,
                        NetSalary = i.BasicSalary,
                        Status = SalaryStatus.Pending,
                        InsertUser = InsertUser,
                        InsertDate = DateTime.UtcNow.ToQatarTime()
                    }).ToList();

                    await _unitOfWork.Repository<Salary>().AddRangeAsync(Data);
                    await _unitOfWork.CompleteAsync();

                    return ApiResponseModel<string>.Success(GenericErrors.GetSuccess);
                }
                else
                    return ApiResponseModel<string>.Failure(GenericErrors.EmployeeNotExist);
            }
            catch
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> PaidEmployeeSalary(List<Salary> Model, DateTime SalaryDate, string UpdateUser)
        {
            try
            {
                var SalaryIds = Model.Select(x => x.SalaryId).Distinct().ToList();
                var EmployeeSalaries = await _unitOfWork.Repository<Salary>().GetAllAsync(x => SalaryIds.Contains(x.SalaryId) && x.SalaryYear == SalaryDate.Year && x.SalaryMonth == SalaryDate.Month);
                if (!EmployeeSalaries.Any())
                    return ApiResponseModel<string>.Failure(GenericErrors.EmployeeNotExist);

                var SalaryDictionary = Model.ToDictionary(x => x.SalaryId);
                var Now = DateTime.UtcNow.ToQatarTime();

                foreach (var employeeSalary in EmployeeSalaries)
                {
                    if (!SalaryDictionary.TryGetValue(employeeSalary.SalaryId, out var salary))
                        continue;

                    employeeSalary.Bonus = salary.Bonus;
                    employeeSalary.Deduction = salary.Deduction;
                    employeeSalary.Advance = salary.Advance;
                    employeeSalary.NetSalary = salary.NetSalary;
                    employeeSalary.Status = SalaryStatus.Paid;
                    employeeSalary.PaidDate = Now;
                    employeeSalary.UpdateUser = UpdateUser;
                    employeeSalary.UpdateDate = Now;
                }

                await _unitOfWork.CompleteAsync();

                return ApiResponseModel<string>.Success(GenericErrors.GetSuccess);
            }
            catch
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> PayrollTransferToExpenses(double TotalAmount, DateTime SalaryDate,string Reason, string InsertUser)
        {
            try
            {
                var Now = DateTime.UtcNow.ToQatarTime();
                var Entity = new Expenses
                {
                    ExpenseCategoryId = 1,
                    Amount = TotalAmount,
                    SalaryMonth = SalaryDate.Month,
                    SalaryYear = SalaryDate.Year,
                    Reason = Reason,
                    ExpenseDate = Now,
                    InsertUser = InsertUser,
                    InsertDate = Now
                };

                await _unitOfWork.Repository<Expenses>().AddAsync(Entity);
                await _unitOfWork.CompleteAsync();

                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }
    }
}
