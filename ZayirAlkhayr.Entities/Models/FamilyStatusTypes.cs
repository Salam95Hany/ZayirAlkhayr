using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "FamilyStatusTypes", Schema = "admin")]
    public class FamilyStatusTypes
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
