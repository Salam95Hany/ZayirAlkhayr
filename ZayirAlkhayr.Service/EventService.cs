using Microsoft.AspNetCore.Hosting;
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

        public List<Event> GetAllEvents()
        {
            var results = _Context.Events.Where(i => i.IsVisible).Select(i => new Event
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                FromDate = i.FromDate,
                ToDate = i.ToDate,
                Image = Path.Combine(ApiLocalUrl, ImageFiles.EventImages.ToString(), i.Image)
            }).ToList();
            return results;
        }

        public async Task<HandleErrorResponseModel> AddNewEvent(Event Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Event = new Event();
                Event.Title = Model.Title;
                Event.Description = Model.Description;
                Event.FromDate = Model.FromDate;
                Event.ToDate = Model.ToDate;
                Event.IsVisible = Model.IsVisible;
                Event.InsertUser = Model.InsertUser;
                Event.InsertDate = DateTime.Now;

                var FileName = await _manageFileService.UploadFile(Model.File, "", ImageFiles.EventImages);
                if (FileName.Done)
                    Event.Image = FileName.StringValue;
                else
                    return FileName;

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

        public async Task<HandleErrorResponseModel> UpdateEvent(Event Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Event = _Context.Events.FirstOrDefault(x => x.Id == Model.Id);
                Event.Title = Model.Title;
                Event.Description = Model.Description;
                Event.FromDate = Model.FromDate;
                Event.ToDate = Model.ToDate;
                Event.IsVisible = Model.IsVisible;
                Event.UpdateUser = Model.UpdateUser;
                Event.UpdateDate = DateTime.Now;

                if (Model.File != null)
                {
                    var FileName = await _manageFileService.UploadFile(Model.File, Model.OldFileName, ImageFiles.EventImages);
                    if (FileName.Done)
                        Event.Image = FileName.StringValue;
                    else
                        return FileName;
                }

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
                    _manageFileService.DeleteFile(Event.Image, ImageFiles.EventImages);
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
    }
}
