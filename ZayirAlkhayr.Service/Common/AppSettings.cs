using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Interface.Common;

namespace ZayirAlkhayr.Service.Common
{
    public class AppSettings : IAppSettings
    {
        public string[] URLList { get; set; }
        public string ApiUrlLocal { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public JWT Jwt { get; set; }

    }
}
