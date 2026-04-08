using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Entities.Specifications.Orders
{
    public class OrderNumberSpecification: BaseSpecification<Order>
    {
        public OrderNumberSpecification(DateTime OrderDate) : base(i => i.InsertDate.Value.ToString("yyyy-MM-dd") == OrderDate.ToString("yyyy-MM-dd"))
        {

        }
    }
}
