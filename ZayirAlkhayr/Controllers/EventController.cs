using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        [HttpGet("GetAllEvents")]
        public List<Event> GetAllEvents()
        {
            var result = _eventService.GetAllEvents();
            return result;
        }

        [HttpPost("AddNewEvent")]
        public async Task<HandleErrorResponseModel> AddNewEvent(Event Model)
        {
            var result = await _eventService.AddNewEvent(Model);
            return result;
        }

        [HttpPost("UpdateEvent")]
        public async Task<HandleErrorResponseModel> UpdateEvent(Event Model)
        {
            var result = await _eventService.UpdateEvent(Model);
            return result;
        }

        [HttpGet("DeleteEvent")]
        public HandleErrorResponseModel DeleteEvent(int EventId)
        {
            var result = _eventService.DeleteEvent(EventId);
            return result;
        }
    }
}
