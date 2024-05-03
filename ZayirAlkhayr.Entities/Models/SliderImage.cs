using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "SliderImages", Schema = "web")]
    public class SliderImage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public string OldFileName { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
    }
}
