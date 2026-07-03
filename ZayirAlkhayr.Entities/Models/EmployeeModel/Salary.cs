using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;

namespace ZayirAlkhayr.Entities.Models.EmployeeModel
{
    [Table(name: "Salaries", Schema = "Emp")]
    public class Salary
    {
        public int SalaryId { get; set; }
        public int EmployeeId { get; set; }
        public int SalaryMonth { get; set; }
        public int SalaryYear { get; set; }
        public double BasicSalary { get; set; } // الراتب الأساسي
        public double Bonus { get; set; } // المكافأة
        public double Deduction { get; set; } // الخصم
        public double Advance { get; set; } // السلف
        public double NetSalary { get; set; } // صافي الراتب
        public SalaryStatus Status { get; set; } // 0 = غير مدفوع, 1 = مدفوع
        public DateTime? PaidDate { get; set; }
        public string PaidBy { get; set; }
        public string Notes { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

        [NotMapped]
        public string EmployeeName { get; set; }
    }
}
