using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs.Orders;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.POS;

namespace ZayirAlkhayr.Controllers.POS
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("GetAllOrders")]
        public async Task<ApiResponseModel<DataTable>> GetAllOrders(PagingFilterModel PagingFilter)
        {
            var results = await _orderService.GetAllOrders(PagingFilter);
            return results;
        }

        [HttpPost("GetAllOrderFilters")]
        public async Task<ApiResponseModel<List<FilterModel>>> GetAllOrderFilters(PagingFilterModel PagingFilter)
        {
            var results = await _orderService.GetAllOrderFilters(PagingFilter);
            return results;
        }

        [HttpGet("GetOrderDetailsByOrderId")]
        public async Task<ApiResponseModel<OrderDetailsWithCustomer>> GetOrderDetailsByOrderId(int OrderId)
        {
            var results = await _orderService.GetOrderDetailsByOrderId(OrderId);
            return results;
        }

        [HttpGet("GetOrderWithDetailsByOrderId")]
        public async Task<ApiResponseModel<OrderWithDetailsResponse>> GetOrderWithDetailsByOrderId(int OrderId)
        {
            var results = await _orderService.GetOrderWithDetailsByOrderId(OrderId);
            return results;
        }

        [HttpPost("AddNewOrder")]
        public async Task<ApiResponseModel<string>> AddNewOrder(OrderWithDetailsDto Model)
        {
            var results = await _orderService.AddNewOrder(Model);
            return results;
        }

        [HttpPost("UpdateOrder")]
        public async Task<ApiResponseModel<string>> UpdateOrder(OrderWithDetailsDto Model)
        {
            var results = await _orderService.UpdateOrder(Model);
            return results;

        }

        [HttpGet("CancelOrder")]
        public async Task<ApiResponseModel<string>> CancelOrder(string VoidReason, string Action, string? VoidNotes, int OrderId)
        {
            var results = await _orderService.CancelOrder(VoidReason, Action, VoidNotes, OrderId);
            return results;
        }

        [HttpGet("GetCustomerOrdersHistory")]
        public async Task<ApiResponseModel<List<OrderWithDetailsResponse>>> GetCustomerOrdersHistory(int CustomerId)
        {
            var results = await _orderService.GetCustomerOrdersHistory(CustomerId);
            return results;
        }
    }
}
