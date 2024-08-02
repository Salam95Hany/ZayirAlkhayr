using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "BeneFactors", Schema = "web")]
    public class BeneFactors
    {
        public int Id { get; set; }
        public int NationalityId { get; set; }
        public int Code { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string Address { get; set; }
        public string FaceBook { get; set; }
        public string Image { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        [NotMapped]
        public string OldFileName { get; set; }
        [NotMapped]
        public string Nationality { get; set; }
        [NotMapped]
        public IFormFile Files { get; set; }
    }
}
