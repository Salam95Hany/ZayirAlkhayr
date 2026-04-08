using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs.Orders;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface.POS
{
    public interface IOrderService
    {
        Task<ApiResponseModel<List<Order>>> GetAllOrders(PagingFilterModel Model);
        Task<ApiResponseModel<List<OrderDetailsResponse>>> GetOrderDetailsByOrderId(int OrderId);
        Task<ApiResponseModel<OrderWithDetailsResponse>> GetOrderWithDetailsByOrderId(int OrderId);
        Task<ApiResponseModel<string>> AddNewOrder(OrderWithDetailsDto dto);
        Task<ApiResponseModel<string>> UpdateOrder(OrderWithDetailsDto order);
        Task<ApiResponseModel<string>> CancelOrder(string VoidReason, string Action, string? VoidNotes, int OrderId);
    }
}
