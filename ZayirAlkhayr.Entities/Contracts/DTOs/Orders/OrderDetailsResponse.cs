using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Entities.Contracts.DTOs.Orders
{
    public class OrderDetailsWithCustomer
    {
        public string VoidReason { get; set; }
        public string Note { get; set; }
        public List<OrderDetailsResponse> OrderDetails { get; set; }
        public CustomerOrderResponse Customer { get; set; }
    }
    public class OrderDetailsResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public double TotalValue { get; set; }

    }

    public class OrderWithDetailsResponse
    {
        public int OrderNumber { get; set; }
        public string Notes { get; set; }
        public string CashierName { get; set; }
        public double? TotalValue { get; set; }
        public OrderTypes OrderType { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public CustomerOrderResponse Customer { get; set; }
        public List<OrderDetailsResponse> OrderDetails { get; set; }
    }

    public class CustomerOrderResponse
    {
        public int? CustomerId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
