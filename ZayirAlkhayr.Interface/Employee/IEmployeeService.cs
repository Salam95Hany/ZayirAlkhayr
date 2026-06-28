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
    public interface IEmployeeService
    {
        Task<ApiResponseModel<DataTable>> GetAllEmployees(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<List<FilterModel>>> GetAllEmployeeFilters(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<List<JobTitle>>> GetAllJobTitle();
        Task<ApiResponseModel<string>> AddNewJobTitle(JobTitle Model);
        Task<ApiResponseModel<string>> AddNewEmployee(ZayirAlkhayr.Entities.Models.EmployeeModel.Employee Model);
        Task<ApiResponseModel<string>> UpdateJobTitle(JobTitle Model);
        Task<ApiResponseModel<string>> UpdateEmployee(ZayirAlkhayr.Entities.Models.EmployeeModel.Employee Model);
        Task<ApiResponseModel<string>> DeleteJobTitle(int JobTitleId);
        Task<ApiResponseModel<string>> DeleteEmployee(int EmployeeId);
    }
}
