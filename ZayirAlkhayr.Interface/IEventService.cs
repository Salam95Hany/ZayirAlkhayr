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
        Task<HandleErrorResponseModel> AddNewEvent(Event Model);
        Task<HandleErrorResponseModel> UpdateEvent(Event Model);
        HandleErrorResponseModel DeleteEvent(int EventId);
    }
}
