using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Contracts.DTOs.Inventory
{
    public class InventoryAdjustmentRequest
    {
        public int InventoryAdjustmentId { get; set; }
        public int InventoryItemId { get; set; }
        public double QuantityChange { get; set; }
        public string Reason { get; set; }
        public string InsertUser { get; set; }
    }
}
