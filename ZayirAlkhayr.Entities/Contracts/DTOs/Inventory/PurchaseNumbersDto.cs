using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Contracts.DTOs.Inventory
{
    public class PurchaseNumbersDto
    {
        public int PurchaseId { get; set; }
        public string PurchaseNumber { get; set; }
        public double RemainingAmount { get; set; }
    }
}
