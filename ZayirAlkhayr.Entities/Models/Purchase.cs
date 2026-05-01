using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "Purchases", Schema = "Inv")]
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public int SupplierId { get; set; }
        public string PurchaseNumber { get; set; }
        public double TotalAmount { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
