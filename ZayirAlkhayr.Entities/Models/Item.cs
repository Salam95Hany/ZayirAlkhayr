using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "Items", Schema = "POS")]
    public class Item
    {
        public int ItemId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double CostPrice { get; set; }
        public bool IsActive { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        [NotMapped]
        public string OldFileName { get; set; }
        [NotMapped]
        public IFormFile Files { get; set; }
    }
}
