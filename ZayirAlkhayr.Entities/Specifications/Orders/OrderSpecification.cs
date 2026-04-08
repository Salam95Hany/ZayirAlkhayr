using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Entities.Specifications.Orders
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification(string SearchText) : base()
        {
            if (!string.IsNullOrEmpty(SearchText))
                AddCriteria(i => i.OrderNumber.ToString().Contains(SearchText));
        }
    }
}
