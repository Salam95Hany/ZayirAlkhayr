using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Common
{
    public class BeneFactorOrphansModel
    {
        public int OrphansId { get; set; }
        public int BenefactorId { get; set; }
        public string BenefactorPhone { get; set; }
        public string BenefactorAddress { get; set; }
        public string BenefactorType { get; set; }
        public bool IsGuaranteed { get; set; }
    }
}
