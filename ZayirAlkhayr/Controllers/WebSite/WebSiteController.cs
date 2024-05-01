using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ZayirAlkhayr.Interface.WebSite;

namespace ZayirAlkhayr.Controllers.WebSite
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebSiteController : ControllerBase
    {
        private readonly IWebSiteService _webSiteService;
        public WebSiteController(IWebSiteService webSiteService)
        {
            _webSiteService = webSiteService;
        }

        
    }
}
