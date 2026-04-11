using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Entities.Specifications.Orders
{
    public class CustomerOrdersSpecification : BaseSpecification<Order>
    {
        public CustomerOrdersSpecification(int CustomerId) : base(i => i.CustomerId == CustomerId)
        {
            AddInclude(i => i.OrderDetails);
            AddInclude(i => i.User);
            AddInclude("OrderDetails.Item");
            AddInclude("OrderDetails.Item.Category");

            ApplyOrderBy(i => i.InsertDate);
        }
    }
}
