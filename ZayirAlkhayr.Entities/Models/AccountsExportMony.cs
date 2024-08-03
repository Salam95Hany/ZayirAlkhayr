using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "AccountsExportMony", Schema = "admin")]
    public class AccountsExportMony
    {
        public int Id { get; set; }
        public int BeneFactorId { get; set; }
        public int BeneFactorTypeId { get; set; }
        public double TotalValue { get; set; }
        public string Details { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
    }
}
