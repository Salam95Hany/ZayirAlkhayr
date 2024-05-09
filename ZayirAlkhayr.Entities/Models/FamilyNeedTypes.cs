using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "FamilyNeedTypes", Schema = "admin")]
    public class FamilyNeedTypes
    {
        public int Id { get; set; }
        public string NeedTypeName { get; set; }
        public string Category { get; set; }
    }
}
