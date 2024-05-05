using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "FamilyIncome", Schema = "admin")]
    public class FamilyIncome
    {
        public int Id { get; set; }
        public int FamilyStatusId { get; set; }
        public int? FatherJop { get; set; }
        public int? MotherJop { get; set; }
        public int? ChildernsJop { get; set; }
        public int? AffairSpension_SocialSolidarity { get; set; } // معاش شؤون\تضامن اجتماعي
        public int? Project { get; set; }
        public int? LiveStock_Lands { get; set; } // اراضي\مواشي
        public int? Organization_ZakatCommittee { get; set; } // جمعية خيرية\جمعية شرعية\لجنة زكاة
        public int? InsurancePension { get; set; } // معاش التامينات
        public string Comments { get; set; }
        public string Other { get; set; }
        public int? TotalFamilyIncome { get; set; }
    }
}
