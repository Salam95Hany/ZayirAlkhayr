using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.WebSite;
using ZayirAlkhayr.Service.WebSite;

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

        [HttpGet("GetWebSiteProjectsById")]
        public Projects GetWebSiteProjectsById(int ProjectId)
        {
            var result = _projectsService.GetWebSiteProjectsById(ProjectId);
            return result;
        }

        [HttpPost("GetAllProjects")]
        public DataTable GetAllProjects(PagingFilterModel PagingFilter)
        {
            var result = _projectsService.GetAllProjects(PagingFilter);
            return result;
        }

        [HttpGet("GetProjectsSliderImagesById")]
        public List<ProjectDetails> GetProjectsSliderImagesById(int ProjectId)
        {
            var result = _projectsService.GetProjectsSliderImagesById(ProjectId);
            return result;
        }

        [HttpPost("AddNewProjects")]
        public HandleErrorResponseModel AddNewProjects(Projects Model)
        {
            var result = _projectsService.AddNewProjects(Model);
            return result;
        }

        [HttpPost("UpdateProjects")]
        public HandleErrorResponseModel UpdateProjects(Projects Model)
        {
            var result = _projectsService.UpdateProjects(Model);
            return result;
        }

        [HttpGet("DeleteProjects")]
        public HandleErrorResponseModel DeleteProjects(int ProjectId)
        {
            var result = _projectsService.DeleteProjects(ProjectId);
            return result;
        }

        [HttpPost("AddProjectsSliderImage")]
        public async Task<HandleErrorResponseModel> AddProjectsSliderImage([FromForm] UploadFileModel Model)
        {
            var result = await _projectsService.AddProjectsSliderImage(Model);
            return result;
        }

        [HttpGet("CheckProjectLinkIsActive")]
        public bool CheckProjectLinkIsActive(int ProjectId)
        {
            var result = _projectsService.CheckProjectLinkIsActive(ProjectId);
            return result;
        }

        [HttpGet("GetAllDeniedProjects")]
        public List<ProjectsDenied> GetAllDeniedProjects()
        {
            var result = _projectsService.GetAllDeniedProjects();
            return result;
        }
    }
}
