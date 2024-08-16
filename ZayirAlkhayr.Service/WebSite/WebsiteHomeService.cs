using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.Common;
using ZayirAlkhayr.Interface.WebSite;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service.WebSite
{
    public class WebsiteHomeService : IWebsiteHomeService
    {
        private readonly ZADbContext _Context;
        private readonly IManageFileService _manageFileService;
        private readonly IConfiguration _configuration;
        private readonly ISQLHelper _sQLHelper;
        private string ApiLocalUrl;
        public WebsiteHomeService(ZADbContext Context, IManageFileService manageFileService, IConfiguration configuration, ISQLHelper sQLHelper)
        {
            _Context = Context;
            _manageFileService = manageFileService;
            _configuration = configuration;
            _sQLHelper = sQLHelper;
            ApiLocalUrl = _configuration["ApiUrlLocal"];
        }

        public DataTable GetHomeSliderImages(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@ApiUrl", ApiLocalUrl);
            Params[2] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[3] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            var dt = _sQLHelper.ExecuteDataTable("web.SP_GetHomeSliderImages", Params);
            return dt;
        }

        public List<FilterModel> GetAllWebPagesFilters(string PageName)
        {
            var Params = new SqlParameter[1];
            Params[0] = new SqlParameter("@PageName", PageName);
            var dt = _sQLHelper.ExecuteDataTable("web.SP_GetAllWebPagesFilter", Params);
            var Filters = _sQLHelper.GroupingFilters(dt);
            return Filters;
        }

        public List<Footer> GetFooterData()
        {
            var results = _Context.Footers.ToList();
            return results;
        }

        public List<PagesAutoSearch> GetPagesAutoSearch(string SearchText)
        {
            if (string.IsNullOrEmpty(SearchText))
                return _Context.PagesAutoSearch.ToList();
            else
                return _Context.PagesAutoSearch.Where(i => i.Name.Contains(SearchText)).ToList();
        }

        public async Task<HandleErrorResponseModel> AddNewSliderImage(SliderImage Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Slider = new SliderImage();
                Slider.Title = Model.Title;
                Slider.IsVisible = Model.IsVisible;
                Slider.InsertUser = Model.InsertUser;
                Slider.InsertDate = DateTime.Now.AddHours(1);

                var FileName = await _manageFileService.UploadFile(Model.Files, "", ImageFiles.SliderImages);
                if (FileName.Done)
                    Slider.Image = FileName.StringValue;
                else
                    return FileName;

                _Context.SliderImages.Add(Slider);
                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم اضافة عنصر جديد بنجاح";
                return Response;
            }
            catch (Exception)
            {
                var Response = new HandleErrorResponseModel();
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public async Task<HandleErrorResponseModel> UpdateSliderImage(SliderImage Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Slider = _Context.SliderImages.FirstOrDefault(x => x.Id == Model.Id);
                Slider.Title = Model.Title;
                Slider.IsVisible = Model.IsVisible;
                Slider.UpdateUser = Model.InsertUser;
                Slider.UpdateDate = DateTime.Now.AddHours(1);

                if (Model.Files != null)
                {
                    var FileName = await _manageFileService.UploadFile(Model.Files, Model.OldFileName, ImageFiles.SliderImages);
                    if (FileName.Done)
                        Slider.Image = FileName.StringValue;
                    else
                        return FileName;
                }

                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم تعديل العنصر بنجاح";
                return Response;
            }
            catch (Exception)
            {
                var Response = new HandleErrorResponseModel();
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public HandleErrorResponseModel DeleteSliderImage(int SliderImageId)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Slider = _Context.SliderImages.FirstOrDefault(i => i.Id == SliderImageId);
                if (Slider != null)
                {
                    _manageFileService.DeleteFile(Slider.Image, ImageFiles.SliderImages);
                    _Context.SliderImages.Remove(Slider);
                    _Context.SaveChanges();
                    Response.Done = true;
                    Response.Message = "تم حذف العنصر بنجاح";
                    return Response;
                }
                else
                {
                    Response.Done = false;
                    Response.Message = "هذا العنصر غير موجود";
                    return Response;
                }

            }
            catch (Exception)
            {
                var Response = new HandleErrorResponseModel();
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public HandleErrorResponseModel AddNewFooterData(Footer Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Footer = new Footer();

                Footer.Phones = Model.Phones;

                _Context.Footers.Add(Footer);
                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم اضافة عنصر جديد بنجاح";
                return Response;
            }
            catch (Exception)
            {
                var Response = new HandleErrorResponseModel();
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public HandleErrorResponseModel UpdateFooterData(Footer Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Footer = _Context.Footers.FirstOrDefault(x => x.Id == Model.Id);
                if (Footer != null)
                {
                    Footer.Phones = Model.Phones;
                    _Context.SaveChanges();

                    Response.Done = true;
                    Response.Message = "تم تعديل العنصر بنجاح";
                    return Response;
                }
                else
                {
                    Response.Done = false;
                    Response.Message = "هذا العنصر غير موجود";
                    return Response;
                }

            }
            catch (Exception)
            {
                var Response = new HandleErrorResponseModel();
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public HandleErrorResponseModel DeleteFooterData(int FooterId)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Footer = _Context.Footers.FirstOrDefault(i => i.Id == FooterId);
                if (Footer != null)
                {
                    _Context.Footers.Remove(Footer);
                    _Context.SaveChanges();
                    Response.Done = true;
                    Response.Message = "تم حذف العنصر بنجاح";
                    return Response;
                }
                else
                {
                    Response.Done = false;
                    Response.Message = "هذا العنصر غير موجود";
                    return Response;
                }

            }
            catch (Exception)
            {
                var Response = new HandleErrorResponseModel();
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public string CreateSessionId()
        {
            try
            {
                var sessionId = Guid.NewGuid().ToString();
                var Visitor = new WebSiteVisitors { SessionId = sessionId, InsertDate = DateTime.Now.AddHours(1) };
                _Context.WebSiteVisitors.Add(Visitor);
                _Context.SaveChanges();
                return sessionId;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
