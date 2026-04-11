using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Interface.Customer;

namespace ZayirAlkhayr.Controllers.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("GetAllCustomers")]
        public async Task<ApiResponseModel<DataTable>> GetAllCustomers(PagingFilterModel PagingFilter)
        {
            var results = await _customerService.GetAllCustomers(PagingFilter);
            return results;
        }

        [HttpGet("GetCustomerBySearchText")]
        public async Task<ApiResponseModel<List<ZayirAlkhayr.Entities.Models.Customer>>> GetCustomerBySearchText(string SearchText)
        {
            var results = await _customerService.GetCustomerBySearchText(SearchText);
            return results;
        }

        [HttpGet("GetCustomerById")]
        public async Task<ApiResponseModel<ZayirAlkhayr.Entities.Models.Customer>> GetCustomerById(int CustomerId)
        {
            var results = await _customerService.GetCustomerById(CustomerId);
            return results;
        }

        [HttpPost("AddNewCustomer")]
        public async Task<ApiResponseModel<int>> AddNewCustomer(ZayirAlkhayr.Entities.Models.Customer Model)
        {
            var results = await _customerService.AddNewCustomer(Model);
            return results;

        }

        [HttpPost("UpdateCustomer")]
        public async Task<ApiResponseModel<string>> UpdateCustomer(ZayirAlkhayr.Entities.Models.Customer Model)
        {
            var results = await _customerService.UpdateCustomer(Model);
            return results;

        }

        [HttpGet("DeleteCustomer")]
        public async Task<ApiResponseModel<string>> DeleteCustomer(int CustomerId)
        {
            var results = await _customerService.DeleteCustomer(CustomerId);
            return results;
        }
    }
}
