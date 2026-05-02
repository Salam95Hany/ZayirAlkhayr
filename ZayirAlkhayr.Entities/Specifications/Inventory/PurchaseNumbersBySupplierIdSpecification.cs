using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Entities.Specifications.Inventory
{
    public class PurchaseNumbersBySupplierIdSpecification:BaseSpecification<Purchase>
    {
        public PurchaseNumbersBySupplierIdSpecification(int SupplierId):base(p => p.SupplierId == SupplierId)
        {
            
        }
    }

    public class SupplierPaymentBySupplierIdSpecification : BaseSpecification<SupplierPayment>
    {
        public SupplierPaymentBySupplierIdSpecification(int SupplierId) : base(p => p.SupplierId == SupplierId)
        {

        }
    }
}
