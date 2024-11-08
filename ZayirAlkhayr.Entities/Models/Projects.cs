using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "Projects", Schema = "web")]
    public class Projects
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TotalDonationAmount { get; set; } // اجمالي مبلغ التبرع
        public string BenefactorCount { get; set; } // عدد المتبرعين
        public string TotalAmount { get; set; } // اجمالي المبلغ
        public string RemainingAmount { get; set; } // باق المبلغ
        public string ProjectUrl { get; set; }
        public bool IsVisible { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        [NotMapped]
        public List<string> Images { get; set; }
    }
}
