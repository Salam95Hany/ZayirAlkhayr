using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "FamilyDetails", Schema = "admin")]
    public class FamilyDetails
    {
        public int Id { get; set; }
        public int FamilyStatusId { get; set; }
        public string Name { get; set; }
        public string Relevance { get; set; } // الصلة
        public int? Age { get; set; }
        public string MaritalStatus { get; set; } // الحالة الاجتماعية
        public string Education { get; set; }
        public string Jop { get; set; }
        public string NationalId { get; set; } // الرقم القومي
        public int? ChildernsCount { get; set; }
        public int? FamilyMembersCount { get; set; } // عدد افراد الاسرة
    }
}
