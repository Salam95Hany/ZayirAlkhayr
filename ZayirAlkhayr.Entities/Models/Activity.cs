using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "Activities", Schema = "web")]
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int? InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsVisible { get; set; }
        [NotMapped]
        public string OldFileName { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        
    }
}
