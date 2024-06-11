using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "EventSliderImages", Schema = "web")]
    public class EventSliderImages
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Image { get; set; }
    }
}
