using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Contracts.DTOs.Inventory
{
    public class SupplierSummaryDto
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime InsertDate { get; set; }
        public int InvoiceCount { get; set; }
        public double TotalPurchases { get; set; }
        public double TotalPaid { get; set; }
        public double RemainingAmount { get; set; }
    }
}
