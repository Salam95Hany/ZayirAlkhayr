using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("AddNewCustomer")]
        public async Task<ApiResponseModel<string>> AddNewCustomer(ZayirAlkhayr.Entities.Models.Customer Model)
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
