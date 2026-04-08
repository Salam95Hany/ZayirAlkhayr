using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Entities.Specifications.Orders
{
    public class OrderDetailsSpecification: BaseSpecification<Order>
    {
        public OrderDetailsSpecification(int OrderId) : base(i => i.OrderId == OrderId)
        {
            AddInclude(i => i.OrderDetails);
            AddInclude("OrderDetails.Product");
            AddInclude("OrderDetails.Product.Category");

        }
    }
}
