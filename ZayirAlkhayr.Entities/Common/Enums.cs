using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Common
{
    public enum ImageFiles
    {
        Items = 0,
        Categories = 1
    }

    public enum OrderTypes
    {
        TakeAway = 1,
        Delivery =2
    }

    public enum OrderStatus
    {
        InProgress = 1,
        Completed = 2,
        Cancelled = 3
    }
}
