using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface
{
    public interface IActivityService
    {
        List<Activity> GetAllActivities();
        List<ActivitySliderImage> GetActivitySliderImagesById(int ActivityId);
        ActivityModel GetActivityWithSliderImagesById(int ActivityId, int RowSize);
        Task<HandleErrorResponseModel> AddNewActivity(Activity Model);
        Task<HandleErrorResponseModel> UpdateActivity(Activity Model);
        HandleErrorResponseModel DeleteActivity(int ActivityId);
        Task<HandleErrorResponseModel> AddActivitySliderImage(IFormFile File, int ActivityId);
        HandleErrorResponseModel DeleteActivitySliderImage(string FileName, int Id);
    }
}
