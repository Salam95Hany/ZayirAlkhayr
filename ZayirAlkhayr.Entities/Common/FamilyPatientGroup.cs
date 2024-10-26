using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Common
{
    public class FamilyPatientGroup
    {
        public int Id { get; set; }
        public int FamilyStatusId { get; set; }
        public string Name { get; set; }
        public List<int> PatientTypeIds { get; set; }
        public string PatientDate { get; set; }
        public string Specialization { get; set; }
        public bool? IsMedicalReport { get; set; }
        public bool? IsNeedProcess { get; set; }
    }
}
