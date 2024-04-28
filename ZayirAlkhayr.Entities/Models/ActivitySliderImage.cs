using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "ActivitiesSliderImage", Schema = "web")]
    public class ActivitySliderImage
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public string Image { get; set; }
    }
}
