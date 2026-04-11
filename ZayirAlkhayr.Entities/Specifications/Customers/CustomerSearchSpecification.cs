using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Entities.Specifications.Customers
{
    public class CustomerSearchSpecification : BaseSpecification<Customer>
    {
        public CustomerSearchSpecification(string SearchText) : base()
        {
            if (!string.IsNullOrEmpty(SearchText))
                AddCriteria(fc => fc.FullName.Contains(SearchText) || fc.Phone.Contains(SearchText));
        }
    }
}
