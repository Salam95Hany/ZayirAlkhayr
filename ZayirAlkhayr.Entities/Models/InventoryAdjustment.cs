using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ZayirAlkhayr.Entities.Common;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "InventoryAdjustments", Schema = "Inv")]
    public class InventoryAdjustment
    {
        public int InventoryAdjustmentId { get; set; }
        public string ActionId { get; set; } // OrderNumber / PurchaseNumber / Manual
        public AdjustmentTypes AdjustmentType { get; set; }
        public string Reason { get; set; }
        public int TotalAffectedItems { get; set; }
        public string InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public ICollection<InventoryAdjustmentDetail> Details { get; set; }
    }
}
