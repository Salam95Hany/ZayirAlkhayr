using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;

namespace ZayirAlkhayr.Interface.Customer
{
    public interface ICustomerService
    {
        Task<ApiResponseModel<string>> AddNewCustomer(ZayirAlkhayr.Entities.Models.Customer Model);
        Task<ApiResponseModel<string>> UpdateCustomer(ZayirAlkhayr.Entities.Models.Customer Model);
        Task<ApiResponseModel<string>> DeleteCustomer(int CustomerId);
    }
}
