using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models.EmployeeModel
{
    [Table(name: "Expenses", Schema = "Emp")]
    public class Expenses
    {
        public int ExpensesId { get; set; }
        public int ExpenseCategoryId { get; set; }
        public double Amount { get; set; }
        public int? SalaryMonth { get; set; }
        public int? SalaryYear { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Reason { get; set; }
        public bool IsActive { get; set; }
        public string InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
