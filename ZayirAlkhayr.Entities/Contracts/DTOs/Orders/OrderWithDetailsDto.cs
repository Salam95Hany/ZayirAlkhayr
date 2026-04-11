using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;

namespace ZayirAlkhayr.Entities.Contracts.DTOs.Orders
{
    public class OrderWithDetailsDto
    {
        public int? OrderId { get; set; }
        public int? CustomerId { get; set; }
        public OrderTypes OrderType { get; set; }
        public double TotalAmount { get; set; }
        public int CostDelivery { get; set; }
        public string Note { get; set; }
        public string UserId { get; set; }
        public List<OrderDetailDto> Details { get; set; }
    }

    public class OrderDetailDto
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
