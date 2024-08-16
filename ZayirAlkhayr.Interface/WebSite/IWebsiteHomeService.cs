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
    public interface IWebsiteHomeService
    {
        DataTable GetHomeSliderImages(PagingFilterModel PagingFilter);
        List<FilterModel> GetAllWebPagesFilters(string PageName);
        List<Footer> GetFooterData();
        List<PagesAutoSearch> GetPagesAutoSearch(string SearchText);
        Task<HandleErrorResponseModel> AddNewSliderImage(SliderImage Model);
        HandleErrorResponseModel AddNewFooterData(Footer Model);
        Task<HandleErrorResponseModel> UpdateSliderImage(SliderImage Model);
        HandleErrorResponseModel UpdateFooterData(Footer Model);
        HandleErrorResponseModel DeleteSliderImage(int SliderImageId);
        HandleErrorResponseModel DeleteFooterData(int FooterId);
        string CreateSessionId();
    }
}
