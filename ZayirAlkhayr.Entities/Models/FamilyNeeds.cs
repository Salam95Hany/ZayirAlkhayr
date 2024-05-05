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
        public int FamilyStatusId { get; set; }
        public string ElectricalAppliances { get; set; } // الاجهزة الكهربائية
        public string Furniture { get; set; } // اثاث
        public string HomeMaintenance { get; set; } // صيانة المنزل
        public string Joinary { get; set; } // نجارة
    }
}
