using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "BeneFactorDetails", Schema = "web")]
    public class BeneFactorDetails
    {
        public int Id { get; set; }
        public int BeneFactorId { get; set; }
        public int? BeneFactorValueId { get; set; }
        public int BeneFactorTypeId { get; set; }
        public string Image { get; set; }
        public string Details { get; set; }
        public double? TotalValue { get; set; }
        public DateTime PaymentDate { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        [NotMapped]
        public string OldFileName { get; set; }
        [NotMapped]
        public bool IsFinalSubscribe { get; set; }
        [NotMapped]
        public IFormFile Files { get; set; }
    }
}
