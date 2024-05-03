using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface;
using ZayirAlkhayr.Interface.Common;

namespace ZayirAlkhayr.Service
{
    public class ActivityService : IActivityService
    {
        private readonly ZADbContext _Context;
        private readonly IManageFileService _manageFileService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private string ApiLocalUrl;
        public ActivityService(ZADbContext Context, IManageFileService manageFileService, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _Context = Context;
            _manageFileService = manageFileService;
            _configuration = configuration;
            ApiLocalUrl = _configuration["ApiUrlLocal"];
            _environment = environment;
        }

        public List<Activity> GetAllActivities()
        {
            var results = _Context.Activities.Where(i => i.IsVisible).Select(i => new Activity
            {
                Id = i.Id,
                Name = i.Name,
                Image = Path.Combine(ApiLocalUrl, ImageFiles.ActivityImages.ToString(), i.Image)
            }).ToList();
            return results;
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

        public ActivityModel GetActivityWithSliderImagesById(int ActivityId, int RowSize)
        {
            var Activity = _Context.Activities.FirstOrDefault(i => i.Id == ActivityId);
            if (Activity == null) { return new ActivityModel(); }
            var ActivitySliderImage = _Context.ActivitiesSliderImage.Where(i => i.ActivityId == ActivityId).ToList();
            var SliderImages = new List<List<string>>();
            bool KeepCalling = true;
            var PageSize = 0;
            while (KeepCalling)
            {
                var results = ActivitySliderImage.Select(i => Path.Combine(ApiLocalUrl, ImageFiles.ActivitySliderImages.ToString(), i.Image)).Skip(PageSize).Take(RowSize).ToList();
                if (results.Count() > 0)
                {
                    SliderImages.Add(results);
                    PageSize += RowSize;
                }
                else
                    KeepCalling = false;
            }
            var ActivityModel = new ActivityModel
            {
                Id = Activity.Id,
                Name = Activity.Name,
                Description = Activity.Description,
                Image = Path.Combine(ApiLocalUrl, ImageFiles.ActivityImages.ToString(), Activity.Image),
                SliderImages = SliderImages
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

                var FileName = await _manageFileService.UploadFile(Model.File, "", ImageFiles.ActivityImages);
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
                ActivityObj.UpdateUser = Model.UpdateUser;
                ActivityObj.UpdateDate = DateTime.Now;

                if (Model.File != null)
                {
                    var FileName = await _manageFileService.UploadFile(Model.File, Model.OldFileName, ImageFiles.ActivityImages);
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

        public async Task<HandleErrorResponseModel> AddActivitySliderImage(IFormFile File, int ActivityId)
        {
            var FileName = await _manageFileService.UploadFile(File, "", ImageFiles.ActivitySliderImages);
            if (FileName.Done)
            {
                var Activity = new ActivitySliderImage();
                Activity.ActivityId = ActivityId;
                Activity.Image = FileName.StringValue;
                _Context.ActivitiesSliderImage.Add(Activity);
                _Context.SaveChanges();
                return FileName;
            }
            else
                return FileName;
        }

        public HandleErrorResponseModel DeleteActivitySliderImage(string FileName, int Id)
        {
            var Activity = _Context.ActivitiesSliderImage.FirstOrDefault(a => a.Id == Id);
            if (Activity != null)
            {
                var File = _manageFileService.DeleteFile(FileName, ImageFiles.ActivitySliderImages);
                if (File.Done)
                {
                    _Context.ActivitiesSliderImage.Remove(Activity);
                    _Context.SaveChanges();
                    return File;
                }
            }

            return new HandleErrorResponseModel() { Done = false, Message = "لقد حدث خطا" };
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
