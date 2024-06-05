using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface
{
    public interface IWebsiteHomeService
    {
        DataTable GetHomeSliderImages();
        List<Footer> GetFooterData();
        Task<HandleErrorResponseModel> AddNewSliderImage(SliderImage Model);
        HandleErrorResponseModel AddNewFooterData(Footer Model);
        Task<HandleErrorResponseModel> UpdateSliderImage(SliderImage Model);
        HandleErrorResponseModel UpdateFooterData(Footer Model);
        HandleErrorResponseModel DeleteSliderImage(int SliderImageId);
        HandleErrorResponseModel DeleteFooterData(int FooterId);
    }
}
