using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "PhotoDetails", Schema = "web")]
    public class PhotoDetails
    {
        public int Id { get; set; }
        public int PhotoId { get; set; }
        public string Image { get; set; }
        public int DisplayOrder { get; set; }
        [NotMapped]
        public string OldFileName { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
    }
}
