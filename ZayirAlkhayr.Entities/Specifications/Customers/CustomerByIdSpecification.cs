using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Entities.Specifications.Customers
{
    public class CustomerByIdSpecification : BaseSpecification<Customer>
    {
        public CustomerByIdSpecification(int CustomerId) : base(i => i.CustomerId == CustomerId)
        {
        }
    }
}
