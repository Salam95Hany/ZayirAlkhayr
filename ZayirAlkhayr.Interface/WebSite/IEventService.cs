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
    public interface IEventService
    {
        DataTable GetAllEvents(PagingFilterModel PagingFilter);
        List<EventGroupingModel> GetAllWebSiteEvents();
        List<EventSliderImages> GetEventSliderImagesById(int EventId);
        HandleErrorResponseModel AddNewEvent(Event Model);
        HandleErrorResponseModel UpdateEvent(Event Model);
        HandleErrorResponseModel DeleteEvent(int EventId);
        Task<HandleErrorResponseModel> AddEventSliderImage(UploadFileModel Model);
        HandleErrorResponseModel ApplyEventFilesSorting(List<FileSortingModel> Model, int EventId);
    }
}
