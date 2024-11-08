using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface.WebSite
{
    public interface IProjectsService
    {
        Projects GetWebSiteProjectsById(int ProjectId);
        DataTable GetAllProjects(PagingFilterModel PagingFilter);
        List<ProjectDetails> GetProjectsSliderImagesById(int ProjectId);
        HandleErrorResponseModel AddNewProjects(Projects Model);
        HandleErrorResponseModel UpdateProjects(Projects Model);
        HandleErrorResponseModel DeleteProjects(int ProjectId);
        Task<HandleErrorResponseModel> AddProjectsSliderImage(UploadFileModel Model);
        bool CheckProjectLinkIsActive(int ProjectId);
        List<ProjectsDenied> GetAllDeniedProjects();
    }
}
