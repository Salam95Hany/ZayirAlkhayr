using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.Common;
using ZayirAlkhayr.Interface.WebSite;

namespace ZayirAlkhayr.Service.WebSite
{
    public class ProjectsService : IProjectsService
    {
        private readonly ZADbContext _Context;
        private readonly IManageFileService _manageFileService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly ISQLHelper _sQLHelper;
        private string ApiLocalUrl;
        private string UiHost;
        public ProjectsService(ZADbContext Context, IManageFileService manageFileService, IConfiguration configuration, IWebHostEnvironment environment, ISQLHelper sQLHelper)
        {
            _Context = Context;
            _manageFileService = manageFileService;
            _configuration = configuration;
            _environment = environment;
            _sQLHelper = sQLHelper;
            ApiLocalUrl = _configuration["ApiUrlLocal"];
            UiHost = _configuration["UiHost"];
        }

        public Projects GetWebSiteProjectsById(int ProjectId)
        {
            var result = _Context.Projects.FirstOrDefault(i => i.Id == ProjectId);
            if (result != null)
            {
                var ProjectImages = _Context.ProjectDetails.Where(x => x.ProjectId == ProjectId).Select(i => Path.Combine(ApiLocalUrl, ImageFiles.ProjectSliderImages.ToString(), i.Image)).ToList();
                result.Images = ProjectImages;
            }

            return result;
        }

        public DataTable GetAllProjects(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Params = new SqlParameter[3];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            var dt = _sQLHelper.ExecuteDataTable("web.SP_GetAllProjects", Params);
            return dt;
        }

        public List<ProjectDetails> GetProjectsSliderImagesById(int ProjectId)
        {
            var results = _Context.ProjectDetails.Where(i => i.ProjectId == ProjectId).Select(i => new ProjectDetails
            {
                Id = i.Id,
                ProjectId = i.ProjectId,
                Image = Path.Combine(ApiLocalUrl, ImageFiles.ProjectSliderImages.ToString(), i.Image),
            }).ToList();
            return results;
        }

        public HandleErrorResponseModel AddNewProjects(Projects Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Project = new Projects();
                var Id = _Context.Projects.Any() ? _Context.Projects.Max(i => i.Id) + 1 : 1;
                Project.Title = Model.Title;
                Project.Description = Model.Description;
                Project.TotalDonationAmount = Model.TotalDonationAmount;
                Project.BenefactorCount = Model.BenefactorCount;
                Project.TotalAmount = Model.TotalAmount;
                Project.RemainingAmount = Model.RemainingAmount;
                Project.ProjectUrl = UiHost + "projects/events/" + Id;
                Project.IsVisible = Model.IsVisible;
                Project.InsertUser = Model.InsertUser;
                Project.InsertDate = DateTime.Now.AddHours(1);

                _Context.Projects.Add(Project);
                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم اضافة مشروع جديد بنجاح";
                return Response;
            }
            catch (Exception)
            {
                var Response = new HandleErrorResponseModel();
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public HandleErrorResponseModel UpdateProjects(Projects Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Project = _Context.Projects.FirstOrDefault(x => x.Id == Model.Id);
                Project.Title = Model.Title;
                Project.Description = Model.Description;
                Project.TotalDonationAmount = Model.TotalDonationAmount;
                Project.BenefactorCount = Model.BenefactorCount;
                Project.TotalAmount = Model.TotalAmount;
                Project.RemainingAmount = Model.RemainingAmount;
                Project.IsVisible = Model.IsVisible;
                Project.UpdateUser = Model.InsertUser;
                Project.UpdateDate = DateTime.Now.AddHours(1);

                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم تعديل المشروع بنجاح";
                return Response;
            }
            catch (Exception)
            {
                var Response = new HandleErrorResponseModel();
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public HandleErrorResponseModel DeleteProjects(int ProjectId)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Project = _Context.Projects.FirstOrDefault(i => i.Id == ProjectId);
                if (Project != null)
                {
                    var SliderImages = _Context.ProjectDetails.Where(i => i.ProjectId == ProjectId).ToList();
                    if (SliderImages.Count > 0)
                        _Context.ProjectDetails.RemoveRange(SliderImages);

                    _Context.Projects.Remove(Project);
                    var ProjectSliderImageNames = SliderImages.Select(i => i.Image).ToList();
                    DeleteProjectsFiles(ProjectSliderImageNames);
                    _Context.SaveChanges();
                    Response.Done = true;
                    Response.Message = "تم حذف المشروع بنجاح";
                    return Response;
                }
                else
                {
                    Response.Done = false;
                    Response.Message = "هذا المشروع غير موجود";
                    return Response;
                }

            }
            catch (Exception)
            {
                var Response = new HandleErrorResponseModel();
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public async Task<HandleErrorResponseModel> AddProjectsSliderImage(UploadFileModel Model)
        {
            var Response = new HandleErrorResponseModel();
            try
            {
                if (Model.Files != null)
                    foreach (var newFile in Model.Files)
                    {
                        var FileName = await _manageFileService.UploadFile(newFile, "", ImageFiles.ProjectSliderImages);
                        if (FileName.Done)
                        {
                            var Project = new ProjectDetails();
                            Project.ProjectId = Model.Id;
                            Project.Image = FileName.StringValue;
                            _Context.ProjectDetails.Add(Project);
                            _Context.SaveChanges();
                        }
                    }

                if (Model.DeletedFiles != null)
                    foreach (var file in Model?.DeletedFiles)
                    {
                        var FileName = _manageFileService.DeleteFile(file.FileName, ImageFiles.ProjectSliderImages);
                        if (FileName.Done)
                        {
                            var Slider = _Context.ProjectDetails.FirstOrDefault(i => i.Id == file.Id);
                            if (Slider != null)
                            {
                                _Context.ProjectDetails.Remove(Slider);
                                _Context.SaveChanges();
                            }
                        }
                    }
                Response.Done = true;
                Response.Message = "تم اضافة الصور بنجاح";
                return Response;
            }
            catch (Exception)
            {
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        private void DeleteProjectsFiles(List<string> ProjectSliderImageNames)
        {
            var ProjectSliderImagePaths = Directory.GetFiles(Path.Combine(_environment.WebRootPath, ImageFiles.ProjectSliderImages.ToString()));

            if (ProjectSliderImagePaths.Count() > 0)
            {
                var Files = ProjectSliderImagePaths.Where(i => ProjectSliderImageNames.Any(x => i.Contains(x))).ToList();
                if (Files.Count() > 0)
                {
                    Files.ForEach(i => File.Delete(i));
                }
            }
        }

        public bool CheckProjectLinkIsActive(int ProjectId)
        {
            var result = _Context.Projects.FirstOrDefault(i => i.Id == ProjectId);
            if (result == null)
                return false;
            else
                return result.IsVisible;
        }

        public List<ProjectsDenied> GetAllDeniedProjects()
        {
            var result = _Context.Projects.Select(i => new ProjectsDenied
            {
                Id = i.Id,
                Name = i.Title,
                Url = i.ProjectUrl,
                IsVisible = i.IsVisible
            }).ToList();

            return result;
        }
    }
}
