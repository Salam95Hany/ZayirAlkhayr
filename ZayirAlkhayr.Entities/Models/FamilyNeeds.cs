using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "FamilyNeeds", Schema = "admin")]
    public class FamilyNeeds
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public int NeedTypeId { get; set; }
    }
}
