using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "InventoryItems", Schema = "Inv")]
    public class InventoryItem
    {
        public int InventoryItemId { get; set; }
        public int UnitId { get; set; }
        public string Name { get; set; }
        public double CurrentQuantity { get; set; }
        public double MinQuantity { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
