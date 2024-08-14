using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "GeneralTasks", Schema = "admin")]
    public class GeneralTasks
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public string Task { get; set; }
        public string AssignTo { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
