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
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;
        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpGet("GetAllActivities")]
        public List<Activity> GetAllActivities()
        {
            var results = _activityService.GetAllActivities();
            return results;
        }

        [HttpGet("GetActivitySliderImagesById")]
        public List<ActivitySliderImage> GetActivitySliderImagesById(int ActivityId)
        {
            var results = _activityService.GetActivitySliderImagesById(ActivityId);
            return results;
        }

        [HttpGet("GetActivityWithSliderImagesById")]
        public ActivityModel GetActivityWithSliderImagesById(int ActivityId, int RowSize)
        {
            var results = _activityService.GetActivityWithSliderImagesById(ActivityId, RowSize);
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
            var results = await _activityService.AddActivitySliderImage(Model.File, Model.Id);
            return results;
        }

        [HttpGet("DeleteActivitySliderImage")]
        public HandleErrorResponseModel DeleteActivitySliderImage(string FileName, int Id)
        {
            var results = _activityService.DeleteActivitySliderImage(FileName, Id);
            return results;
        }
    }
}
