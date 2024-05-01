using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.WebSite;

namespace ZayirAlkhayr.Service.WebSite
{
    public class WebSiteService: IWebSiteService
    {
        private readonly ZADbContext _Context;
        public WebSiteService(ZADbContext Context)
        {
            _Context = Context;
        }
    }
}
