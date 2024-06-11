using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface
{
    public interface IEventService
    {
        List<Event> GetAllEvents();
        List<EventGroupingModel> GetAllWebSiteEvents();
        List<EventSliderImages> GetEventSliderImagesById(int EventId);
        HandleErrorResponseModel AddNewEvent(Event Model);
        HandleErrorResponseModel UpdateEvent(Event Model);
        HandleErrorResponseModel DeleteEvent(int EventId);
        Task<HandleErrorResponseModel> AddEventSliderImage(UploadFileModel Model);
    }
}
