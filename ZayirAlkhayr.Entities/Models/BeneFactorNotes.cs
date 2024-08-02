using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "BeneFactorNotes", Schema = "web")]
    public class BeneFactorNotes
    {
        public int Id { get; set; }
        public int BeneFactorId { get; set; }
        public string Note { get; set; }
        public string Suggestion { get; set; }
        public DateTime? InsertDate { get; set; }
    }
}
