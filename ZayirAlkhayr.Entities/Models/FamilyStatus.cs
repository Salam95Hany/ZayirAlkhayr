using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "FamilyStatus", Schema = "admin")]
    public class FamilyStatus
    {
        public int Id { get; set; }
        public int StatusTypeId { get; set; }
        public int CategoryId { get; set; }
        public int NationalityId { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string Fname { get; set; }
        public string Address { get; set; }
        public string Village { get; set; }
        public string Center { get; set; }
        public string Governorate { get; set; }
        public string Phone { get; set; }
        public string Phone1 { get; set; }
        public string SupportingParty { get; set; }
        public string ReasonOfRefuse { get; set; } // سبب الرفض
        public DateTime AddedDate { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
