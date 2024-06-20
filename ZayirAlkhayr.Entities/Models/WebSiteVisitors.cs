using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "WebSiteVisitors", Schema = "web")]
    public class WebSiteVisitors
    {
        public int Id { get; set; }
        public string SessionId { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
