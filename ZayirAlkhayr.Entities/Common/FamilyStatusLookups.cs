using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Entities.Common
{
    public class FamilyStatusLookups
    {
        public List<FamilyCategories> Categories { get; set; }
        public List<FamilyNationalities> Nationalities { get; set; }
        public List<FamilyNeedTypes> FamilyNeeds { get; set; }
        public List<FamilyNeedCategories> FamilyNeedCategories { get; set; }
        public List<FamilyStatusTypes> StatusTypes { get; set; }
        public List<FamilyPatientTypes> PatientTypes { get; set; }

    }

    public class UpdateFamilyStatusLookups
    {
        public FamilyStatusLookups Lookups { get; set; }
        public FamilyStatus FamilyStatus { get; set; }
        public FamilyIncome FamilyIncome { get; set; }
        public FamilyExpenses FamilyExpenses { get; set; }
        public FamilyExtraDetails FamilyExtraDetails { get; set; }
        public List<FamilyDetails> FamilyDetails { get; set; }
        public List<FamilyPatientGroup> FamilyPatient { get; set; }
        public List<FamilyNeeds> FamilyNeeds { get; set; }
    }
}
