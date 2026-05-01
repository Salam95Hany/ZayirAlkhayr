using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "InventoryAdjustmentDetails", Schema = "Inv")]
    public class InventoryAdjustmentDetail
    {
        public int InventoryAdjustmentDetailId { get; set; }
        public int InventoryAdjustmentId { get; set; }
        public InventoryAdjustment InventoryAdjustment { get; set; }
        public int InventoryItemId { get; set; }
        public InventoryItem InventoryItem { get; set; }
        public double QuantityBefore { get; set; }
        public double QuantityAfter { get; set; }
        public double QuantityChange { get; set; }
    }
}
