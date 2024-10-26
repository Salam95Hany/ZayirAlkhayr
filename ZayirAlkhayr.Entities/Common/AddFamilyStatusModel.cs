using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Entities.Common
{
    public class AddFamilyStatusModel
    {
        public FamilyStatus FamilyStatus { get; set; }
        public FamilyIncome FamilyIncome { get; set; }
        public FamilyExpenses FamilyExpenses { get; set; }
        public FamilyExtraDetails FamilyExtraDetails { get; set; }
        public List<FamilyDetails> FamilyDetails { get; set; }
        public List<FamilyPatientGroup> FamilyPatient { get; set; }
        public List<FamilyNeeds> FamilyNeeds { get; set; }
    }
}
