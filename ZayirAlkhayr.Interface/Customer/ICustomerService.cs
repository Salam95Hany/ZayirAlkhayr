using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;

namespace ZayirAlkhayr.Interface.Customer
{
    public interface ICustomerService
    {
        Task<ApiResponseModel<DataTable>> GetAllCustomers(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<List<ZayirAlkhayr.Entities.Models.Customer>>> GetCustomerBySearchText(string SearchText);
        Task<ApiResponseModel<ZayirAlkhayr.Entities.Models.Customer>> GetCustomerById(int CustomerId);
        Task<ApiResponseModel<int>> AddNewCustomer(ZayirAlkhayr.Entities.Models.Customer Model);
        Task<ApiResponseModel<string>> UpdateCustomer(ZayirAlkhayr.Entities.Models.Customer Model);
        Task<ApiResponseModel<string>> DeleteCustomer(int CustomerId);
    }
}
