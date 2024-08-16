using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.WebSite;
using ZayirAlkhayr.Service;

namespace ZayirAlkhayr.Controllers.WebSite
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebsiteHomeController : ControllerBase
    {
        private readonly IWebsiteHomeService _websiteHomeService;

        public WebsiteHomeController(IWebsiteHomeService websiteHomeService)
        {
            _websiteHomeService = websiteHomeService;
        }

        [HttpPost("GetHomeSliderImages")]
        public DataTable GetHomeSliderImages(PagingFilterModel PagingFilter)
        {
            var result = _websiteHomeService.GetHomeSliderImages(PagingFilter);
            return result;
        }

        [HttpGet("GetAllWebPagesFilters")]
        public List<FilterModel> GetAllWebPagesFilters(string PageName)
        {
            var result = _websiteHomeService.GetAllWebPagesFilters(PageName);
            return result;
        }

        [HttpGet("GetFooterData")]
        public List<Footer> GetFooterData()
        {
            var result = _websiteHomeService.GetFooterData();
            return result;
        }

        [HttpGet("GetPagesAutoSearch")]
        public List<PagesAutoSearch> GetPagesAutoSearch(string SearchText)
        {
            var result = _websiteHomeService.GetPagesAutoSearch(SearchText);
            return result;
        }

        [HttpPost("AddNewSliderImage")]
        public async Task<HandleErrorResponseModel> AddNewSliderImage([FromForm] SliderImage Model)
        {
            var result = await _websiteHomeService.AddNewSliderImage(Model);
            return result;
        }

        [HttpPost("AddNewFooterData")]
        public HandleErrorResponseModel AddNewFooterData(Footer Model)
        {
            var result = _websiteHomeService.AddNewFooterData(Model);
            return result;
        }

        [HttpPost("UpdateSliderImage")]
        public async Task<HandleErrorResponseModel> UpdateSliderImage([FromForm] SliderImage Model)
        {
            var result = await _websiteHomeService.UpdateSliderImage(Model);
            return result;
        }

        [HttpPost("UpdateFooterData")]
        public HandleErrorResponseModel UpdateFooterData(Footer Model)
        {
            var result = _websiteHomeService.UpdateFooterData(Model);
            return result;
        }

        [HttpGet("DeleteSliderImage")]
        public HandleErrorResponseModel DeleteSliderImage(int SliderImageId)
        {
            var result = _websiteHomeService.DeleteSliderImage(SliderImageId);
            return result;
        }

        [HttpGet("DeleteFooterData")]
        public HandleErrorResponseModel DeleteFooterData(int FooterId)
        {
            var result = _websiteHomeService.DeleteFooterData(FooterId);
            return result;
        }

        [HttpGet("CreateSessionId")]
        public object CreateSessionId()
        {
            var result = _websiteHomeService.CreateSessionId();
            return new { SessionId = result };
        }
    }
}
