using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "InventoryAdjustments", Schema = "Inv")]
    public class InventoryAdjustment
    {
        public int InventoryAdjustmentId { get; set; }
        public int InventoryItemId { get; set; }
        public int QuantityChange { get; set; }
        public string Reason { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
