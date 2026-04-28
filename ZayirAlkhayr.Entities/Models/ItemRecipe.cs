using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "ItemRecipes", Schema = "Inv")]
    public class ItemRecipe
    {
        public int ItemRecipeId { get; set; }
        public int ItemId { get; set; }
        public int InventoryItemId { get; set; }
        public double QuantityNeeded { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public virtual Item Item { get; set; }
        public virtual InventoryItem InventoryItem { get; set; }
    }
}
