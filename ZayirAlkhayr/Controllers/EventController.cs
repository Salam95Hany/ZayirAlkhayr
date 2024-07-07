using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface;

namespace ZayirAlkhayr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("GetAllWebSiteEvents")]
        public List<EventGroupingModel> GetAllWebSiteEvents()
        {
            var result = _eventService.GetAllWebSiteEvents();
            return result;
        }

        [HttpPost("GetAllEvents")]
        public DataTable GetAllEvents(PagingFilterModel PagingFilter)
        {
            var result = _eventService.GetAllEvents(PagingFilter);
            return result;
        }

        [HttpGet("GetEventSliderImagesById")]
        public List<EventSliderImages> GetEventSliderImagesById(int EventId)
        {
            var result = _eventService.GetEventSliderImagesById(EventId);
            return result;
        }

        [HttpPost("AddNewEvent")]
        public HandleErrorResponseModel AddNewEvent(Event Model)
        {
            var result = _eventService.AddNewEvent(Model);
            return result;
        }

        [HttpPost("UpdateEvent")]
        public HandleErrorResponseModel UpdateEvent(Event Model)
        {
            var result = _eventService.UpdateEvent(Model);
            return result;
        }

        [HttpGet("DeleteEvent")]
        public HandleErrorResponseModel DeleteEvent(int EventId)
        {
            var result = _eventService.DeleteEvent(EventId);
            return result;
        }

        [HttpPost("AddEventSliderImage")]
        public async Task<HandleErrorResponseModel> AddEventSliderImage([FromForm] UploadFileModel Model)
        {
            var result = await _eventService.AddEventSliderImage(Model);
            return result;
        }
    }
}
