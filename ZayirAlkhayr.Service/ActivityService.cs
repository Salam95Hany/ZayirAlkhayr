using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface;
using ZayirAlkhayr.Interface.Common;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service
{
    public class ActivityService : IActivityService
    {
        private readonly ZADbContext _Context;
        private readonly IManageFileService _manageFileService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly ISQLHelper _sQLHelper;
        private string ApiLocalUrl;
        public ActivityService(ZADbContext Context, IManageFileService manageFileService, IConfiguration configuration, IWebHostEnvironment environment, ISQLHelper sQLHelper)
        {
            _Context = Context;
            _manageFileService = manageFileService;
            _configuration = configuration;
            _environment = environment;
            _sQLHelper = sQLHelper;
            ApiLocalUrl = _configuration["ApiUrlLocal"];
        }

        public DataTable GetAllActivities(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@ApiUrl", ApiLocalUrl);
            Params[2] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[3] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            var dt = _sQLHelper.ExecuteDataTable("web.SP_GetAllActivities", Params);
            return dt;
        }

        public List<ActivitySliderImage> GetActivitySliderImagesById(int ActivityId)
        {
            var results = _Context.ActivitiesSliderImage.Where(i => i.ActivityId == ActivityId).Select(i => new ActivitySliderImage
            {
                Id = i.Id,
                ActivityId = i.ActivityId,
                Image = Path.Combine(ApiLocalUrl, ImageFiles.ActivitySliderImages.ToString(), i.Image)
            }).ToList();
            return results;
        }

        public ActivityModel GetActivityWithSliderImagesById(int ActivityId)
        {
            var Activity = _Context.Activities.FirstOrDefault(i => i.Id == ActivityId);
            if (Activity == null) { return new ActivityModel(); }
            var ActivitySliderImage = _Context.ActivitiesSliderImage.Where(i => i.ActivityId == ActivityId).ToList();
            
            var ActivityModel = new ActivityModel
            {
                Id = Activity.Id,
                Name = Activity.Name,
                Description = Activity.Description,
                SliderImages = ActivitySliderImage.Select(i => Path.Combine(ApiLocalUrl, ImageFiles.ActivitySliderImages.ToString(), i.Image)).ToList()
            };
            return ActivityModel;
        }

        public async Task<HandleErrorResponseModel> AddNewActivity(Activity Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var ActivityObj = new Activity();
                ActivityObj.Name = Model.Name;
                ActivityObj.Description = Model.Description;
                ActivityObj.IsVisible = Model.IsVisible;
                ActivityObj.InsertUser = Model.InsertUser;
                ActivityObj.InsertDate = DateTime.Now;

                var FileName = await _manageFileService.UploadFile(Model.Files, "", ImageFiles.ActivityImages);
                if (FileName.Done)
                    ActivityObj.Image = FileName.StringValue;
                else
                    return FileName;

                _Context.Activities.Add(ActivityObj);
                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم اضافة نشاط جديد بنجاح";
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

        public async Task<HandleErrorResponseModel> UpdateActivity(Activity Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var ActivityObj = _Context.Activities.FirstOrDefault(x => x.Id == Model.Id);
                ActivityObj.Name = Model.Name;
                ActivityObj.Description = Model.Description;
                ActivityObj.IsVisible = Model.IsVisible;
                ActivityObj.UpdateUser = Model.InsertUser;
                ActivityObj.UpdateDate = DateTime.Now;

                if (Model.Files != null)
                {
                    var FileName = await _manageFileService.UploadFile(Model.Files, Model.OldFileName, ImageFiles.ActivityImages);
                    if (FileName.Done)
                        ActivityObj.Image = FileName.StringValue;
                    else
                        return FileName;
                }

                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم تعديل النشاط بنجاح";
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

        public HandleErrorResponseModel DeleteActivity(int ActivityId)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Activity = _Context.Activities.FirstOrDefault(i => i.Id == ActivityId);
                if (Activity != null)
                {
                    var SliderImages = _Context.ActivitiesSliderImage.Where(i => i.ActivityId == ActivityId).ToList();
                    if (SliderImages.Count > 0)
                        _Context.ActivitiesSliderImage.RemoveRange(SliderImages);

                    _Context.Activities.Remove(Activity);
                    var ActivitySliderImageNames = SliderImages.Select(i => i.Image).ToList();
                    DeleteActivityFiles(Activity.Image, ActivitySliderImageNames);
                    _Context.SaveChanges();
                    Response.Done = true;
                    Response.Message = "تم حذف النشاط بنجاح";
                    return Response;
                }
                else
                {
                    Response.Done = false;
                    Response.Message = "هذا النشاط غير موجود";
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

        public async Task<HandleErrorResponseModel> AddActivitySliderImage(UploadFileModel Model)
        {
            var Response = new HandleErrorResponseModel();
            try
            {
                if (Model.Files != null)
                    foreach (var newFile in Model.Files)
                    {
                        var FileName = await _manageFileService.UploadFile(newFile, "", ImageFiles.ActivitySliderImages);
                        if (FileName.Done)
                        {
                            var Activity = new ActivitySliderImage();
                            Activity.ActivityId = Model.Id;
                            Activity.Image = FileName.StringValue;
                            _Context.ActivitiesSliderImage.Add(Activity);
                            _Context.SaveChanges();
                        }
                    }

                if (Model.DeletedFiles != null)
                    foreach (var file in Model?.DeletedFiles)
                    {
                        var FileName = _manageFileService.DeleteFile(file.FileName, ImageFiles.ActivitySliderImages);
                        if (FileName.Done)
                        {
                            var Slider = _Context.ActivitiesSliderImage.FirstOrDefault(i => i.Id == file.Id);
                            if (Slider != null)
                            {
                                _Context.ActivitiesSliderImage.Remove(Slider);
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

        private void DeleteActivityFiles(string ActivityImageName, List<string> ActivitySliderImageNames)
        {
            var ActivityImagePaths = Directory.GetFiles(Path.Combine(_environment.WebRootPath, ImageFiles.ActivityImages.ToString()));
            var ActivitySliderImagePaths = Directory.GetFiles(Path.Combine(_environment.WebRootPath, ImageFiles.ActivitySliderImages.ToString()));

            if (ActivityImagePaths.Count() > 0)
            {
                var File = ActivityImagePaths.FirstOrDefault(i => i.Contains(ActivityImageName));
                if (File != null)
                    System.IO.File.Delete(File);
            }

            if (ActivitySliderImagePaths.Count() > 0)
            {
                var Files = ActivitySliderImagePaths.Where(i => ActivitySliderImageNames.Any(x => i.Contains(x))).ToList();
                if (Files.Count() > 0)
                {
                    Files.ForEach(i => System.IO.File.Delete(i));
                }
            }
        }
    }
}
