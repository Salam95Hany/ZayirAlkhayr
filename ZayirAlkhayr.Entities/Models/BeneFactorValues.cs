using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "BeneFactorValues", Schema = "web")]
    public class BeneFactorValues
    {
        public int Id { get; set; }
        public int BeneFactorId { get; set; }
        public int BeneFactorTypeId { get; set; }
        public double? TotalValue { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsActive { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
