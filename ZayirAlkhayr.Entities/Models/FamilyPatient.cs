using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "FamilyPatient", Schema = "admin")]
    public class FamilyPatient
    {
        public int Id { get; set; }
        public int FamilyStatusId { get; set; }
        public string Name { get; set; }
        public int PatientTypeId { get; set; }
        public string PatientDate { get; set; }
        public string Specialization { get; set; } // التخصص
        public bool? IsMedicalReport { get; set; } // هل يوجد تقرير طبي
        public bool? IsNeedProcess { get; set; } // هل محتاج لعملية
    }
}
