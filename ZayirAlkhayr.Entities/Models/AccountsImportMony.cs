using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    [Table(name: "AccountsImportMony", Schema = "admin")]
    public class AccountsImportMony
    {
        public int Id { get; set; }
        public int BeneFactorId { get; set; }
        public int BeneFactorTypeId { get; set; }
        public double TotalValue { get; set; }
        public string Details { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
