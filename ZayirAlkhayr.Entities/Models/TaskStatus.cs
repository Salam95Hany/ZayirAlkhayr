using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "TaskStatus", Schema = "admin")]
    public class TaskStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
