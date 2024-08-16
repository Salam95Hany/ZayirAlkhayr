using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.WebSite;

namespace ZayirAlkhayr.Controllers.WebSite
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;
        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpPost("GetAllActivities")]
        public DataTable GetAllActivities(PagingFilterModel PagingFilter)
        {
            var results = _activityService.GetAllActivities(PagingFilter);
            return results;
        }

        [HttpGet("GetActivitySliderImagesById")]
        public List<ActivitySliderImage> GetActivitySliderImagesById(int ActivityId)
        {
            var results = _activityService.GetActivitySliderImagesById(ActivityId);
            return results;
        }

        [HttpGet("GetActivityWithSliderImagesById")]
        public ActivityModel GetActivityWithSliderImagesById(int ActivityId)
        {
            var results = _activityService.GetActivityWithSliderImagesById(ActivityId);
            return results;
        }

        [HttpPost("AddNewActivity")]
        public async Task<HandleErrorResponseModel> AddNewActivity([FromForm] Activity Model)
        {
            var results = await _activityService.AddNewActivity(Model);
            return results;
        }

        [HttpPost("UpdateActivity")]
        public async Task<HandleErrorResponseModel> UpdateActivity([FromForm] Activity Model)
        {
            var results = await _activityService.UpdateActivity(Model);
            return results;
        }

        [HttpGet("DeleteActivity")]
        public HandleErrorResponseModel DeleteActivity(int ActivityId)
        {
            var results = _activityService.DeleteActivity(ActivityId);
            return results;
        }

        [HttpPost("AddActivitySliderImage")]
        public async Task<HandleErrorResponseModel> AddActivitySliderImage([FromForm] UploadFileModel Model)
        {
            var results = await _activityService.AddActivitySliderImage(Model);
            return results;
        }
    }
}
