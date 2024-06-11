using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Entities.Common
{
    public class EventGroupingModel
    {
        public string Month { get; set; }
        public List<Event> Events { get; set; }
    }
}
