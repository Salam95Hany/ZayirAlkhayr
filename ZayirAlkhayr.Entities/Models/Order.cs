using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Auth;
using ZayirAlkhayr.Entities.Common;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "Orders", Schema = "POS")]
    public class Order
    {
        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public int OrderNumber { get; set; }
        public OrderTypes OrderType { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public double TotalAmount { get; set; }
        public double? CostDelivery { get; set; }
        public bool IsUpdated { get; set; }
        public string Note { get; set; }
        public string VoidReason { get; set; }
        public string VoidNotes { get; set; }
        public int? TableId { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Customer Customers { get; set; }
        [ForeignKey(nameof(InsertUser))]
        public AdminUser User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
