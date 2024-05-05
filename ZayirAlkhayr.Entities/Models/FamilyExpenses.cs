using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "FamilyExpenses", Schema = "admin")]
    public class FamilyExpenses
    {
        public int Id { get; set; }
        public int FamilyStatusId { get; set; }
        public int? Rent_Electricity_Water_Gas_Sewage { get; set; }
        public int? MedicalExamination_Treatment { get; set; } // كشف طبي\علاج
        public int SchoolExpenses { get; set; } // مصاريف مدارس\حضانة\دروس
        public int? PhysiotherapySessions { get; set; } // جلسات علاج طبيعي
        public int? Analysis { get; set; } // تحاليل
        public int? SatisfactoryTransfers { get; set; } // انتقالات مرضية
        public int? MedicalXRays { get; set; } // اشعة طبية
        public bool?  IsMinisterialSupply { get; set; } // ستفيد من التموين الوزاري
        public bool? IsFoodBank { get; set; } // يستفيد من بنك الطعام
        public int? TotalFamilyExpenses { get; set; } // اجمالي مصروفات الاسرة
        public int? NetFamilyIncome { get; set; } // صافي الدخل
        public int? FamilyCount { get; set; } // عدد الافراد
        public int? AvgPersonIncome { get; set; } // متوسط دخل الفرد
    }
}
