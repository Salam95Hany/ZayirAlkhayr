using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Entities.Specifications.Orders
{
    public class Top3TodayOrdersSpecification:BaseSpecification<Order>
    {
        public Top3TodayOrdersSpecification(bool Igonre): base(o => o.InsertDate >= DateTime.Today && o.InsertDate<DateTime.Today.AddDays(1))
        {
            if (Igonre)
            {
                ApplyOrderByDescending(o => o.InsertDate);
                ApplyPaging(0, 5);
            }
        }
    }
}
