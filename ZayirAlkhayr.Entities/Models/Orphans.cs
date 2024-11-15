using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "Orphans", Schema = "admin")]
    public class Orphans
    {
        public int Id { get; set; }
        public int FamilyStatusId { get; set; }
        public int FamilyDetailsId { get; set; }
        public int? BenefactorId { get; set; }
        public int? NationalityId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FamilyName { get; set; }
        public string Jop { get; set; }
        public string NationalId { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Income { get; set; } // الدخل
        public string AcademicStage { get; set; } // المرحلة الدراسية
        public string FamilyMembersCount { get; set; }
        public string RankingBrothers { get; set; }// ترتيبه بين اخوته
        public string HealthStatus { get; set; } // الحالة الصحية
        public string Notes { get; set; }
        public string BenefactorPhone { get; set; }
        public string BenefactorAddress { get; set; }
        public string BenefactorType { get; set; }
        public bool IsGuaranteed { get; set; } // مكفول
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }        
    }
}
