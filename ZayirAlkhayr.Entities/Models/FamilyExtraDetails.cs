using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "FamilyExtraDetails", Schema = "admin")]
    public class FamilyExtraDetails
    {
        public int Id { get; set; }
        public int FamilyStatusId { get; set; }
        public string StatusDescription { get; set; } // وصف الحالة
        public string HousingNeedsAndStatus { get; set; } // احتياجات السكن والحالة
        public string ResearcherNotes { get; set; } // ملاحظات الباحث
        public string ReferencesNotes { get; set; } // ملاحظات المراجع
        public DateTime? LastVisitDate { get; set; } // تاريخ اخر زيارة
        public string PersonalPapers { get; set; } // الاوراق الشخصية


        public bool AreAllPropertiesDefault()
        {
            var excludedProperties = new List<string> { "Id", "FamilyStatusId" };

            foreach (var property in this.GetType().GetProperties())
            {
                if (excludedProperties.Contains(property.Name))
                    continue;

                var value = property.GetValue(this);
                if (property.PropertyType == typeof(int))
                {
                    if ((int)value != 0) return false;
                }
                else if (property.PropertyType == typeof(string))
                {
                    if (!string.IsNullOrEmpty((string)value)) return false;
                }
                else if (property.PropertyType == typeof(DateTime?))
                {
                    if (value != null) return false;
                }
            }

            return true;
        }
    }
}
