using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class EventService : IEventService
    {
        private readonly ZADbContext _Context;
        private readonly IManageFileService _manageFileService;
        private readonly IConfiguration _configuration;
        private string ApiLocalUrl;
        public EventService(ZADbContext Context, IManageFileService manageFileService, IConfiguration configuration)
        {
            _Context = Context;
            _manageFileService = manageFileService;
            _configuration = configuration;
            ApiLocalUrl = _configuration["ApiUrlLocal"];
        }

        public List<EventGroupingModel> GetAllWebSiteEvents()
        {
            var result = _Context.Events.ToList();
            var Grouping = result.Where(i => i.IsVisible).GroupBy(g => g.Month).Select(group => new EventGroupingModel
            {
                Month = group.Key.ToString("MMMM yyyy", new CultureInfo("ar-AE")),
                Events = group.Select(i => new Event
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    FromDate = i.FromDate,
                    ToDate = i.ToDate,
                    Images = _Context.EventSliderImages.Where(x => x.EventId == i.Id).Select(i => Path.Combine(ApiLocalUrl, ImageFiles.EventSliderImages.ToString(), i.Image)).ToList(),

                }).OrderByDescending(o => o.InsertDate).ToList(),
            }).OrderBy(o => o.Month).ToList();

            return Grouping;
        }

        public List<Event> GetAllEvents()
        {
            var results = _Context.Events.Select(i => new Event
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                FromDate = i.FromDate,
                ToDate = i.ToDate,
                InsertDate = i.InsertDate,
                Month = i.Month,
                IsVisible = i.IsVisible
            }).ToList();
            return results;
        }

        public List<EventSliderImages> GetEventSliderImagesById(int EventId)
        {
            var results = _Context.EventSliderImages.Where(i => i.EventId == EventId).Select(i => new EventSliderImages
            {
                Id = i.Id,
                EventId = i.EventId,
                Image = Path.Combine(ApiLocalUrl, ImageFiles.EventSliderImages.ToString(), i.Image)
            }).ToList();
            return results;
        }

        public HandleErrorResponseModel AddNewEvent(Event Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Event = new Event();
                Event.Title = Model.Title;
                Event.Description = Model.Description;
                Event.FromDate = Model.FromDate;
                Event.ToDate = Model.ToDate;
                Event.Month = Model.Month;
                Event.IsVisible = Model.IsVisible;
                Event.InsertUser = Model.InsertUser;
                Event.InsertDate = DateTime.Now;

                _Context.Events.Add(Event);
                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم اضافة فعالية جديدة بنجاح";
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

        public HandleErrorResponseModel UpdateEvent(Event Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Event = _Context.Events.FirstOrDefault(x => x.Id == Model.Id);
                Event.Title = Model.Title;
                Event.Description = Model.Description;
                Event.FromDate = Model.FromDate;
                Event.ToDate = Model.ToDate;
                Event.Month = Model.Month;
                Event.IsVisible = Model.IsVisible;
                Event.UpdateUser = Model.UpdateUser;
                Event.UpdateDate = DateTime.Now;

                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم تعديل الفعالية بنجاح";
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

        public HandleErrorResponseModel DeleteEvent(int EventId)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Event = _Context.Events.FirstOrDefault(i => i.Id == EventId);
                if (Event != null)
                {
                    _Context.Events.Remove(Event);
                    _Context.SaveChanges();
                    Response.Done = true;
                    Response.Message = "تم حذف الفعالية بنجاح";
                    return Response;
                }
                else
                {
                    Response.Done = false;
                    Response.Message = "هذه الفعالية غير موجودة";
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

        public async Task<HandleErrorResponseModel> AddEventSliderImage(UploadFileModel Model)
        {
            var Response = new HandleErrorResponseModel();
            try
            {
                if (Model.Files != null)
                    foreach (var newFile in Model.Files)
                    {
                        var FileName = await _manageFileService.UploadFile(newFile, "", ImageFiles.EventSliderImages);
                        if (FileName.Done)
                        {
                            var Event = new EventSliderImages();
                            Event.EventId = Model.Id;
                            Event.Image = FileName.StringValue;
                            _Context.EventSliderImages.Add(Event);
                            _Context.SaveChanges();
                        }
                    }

                if (Model.DeletedFiles != null)
                    foreach (var file in Model?.DeletedFiles)
                    {
                        var FileName = _manageFileService.DeleteFile(file.FileName, ImageFiles.EventSliderImages);
                        if (FileName.Done)
                        {
                            var Slider = _Context.EventSliderImages.FirstOrDefault(i => i.Id == file.Id);
                            if (Slider != null)
                            {
                                _Context.EventSliderImages.Remove(Slider);
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
    }
}
