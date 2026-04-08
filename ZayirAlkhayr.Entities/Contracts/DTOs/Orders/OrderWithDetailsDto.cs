using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Contracts.DTOs.Orders
{
    public class OrderWithDetailsDto
    {
        public int? OrderId { get; set; }
        public int? CustomerID { get; set; }
        public double TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string Note { get; set; }
        public double Tax { get; set; }
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
