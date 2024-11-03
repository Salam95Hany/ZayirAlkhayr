using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZayirAlkhayr.Interface.WebSite;

namespace ZayirAlkhayr.Controllers.WebSite
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsService _projectsService;
        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }
    }
}
