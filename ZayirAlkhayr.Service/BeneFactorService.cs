using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface;
using ZayirAlkhayr.Interface.Common;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service
{
    public class BeneFactorService : IBeneFactorService
    {
        private readonly ZADbContext _Context;
        private readonly ISQLHelper _sQLHelper;
        private readonly IConfiguration _configuration;
        private readonly IManageFileService _manageFileService;
        private string ApiLocalUrl;
        public BeneFactorService(ZADbContext context, ISQLHelper sQLHelper, IConfiguration configuration, IManageFileService manageFileService)
        {
            _Context = context;
            _sQLHelper = sQLHelper;
            _configuration = configuration;
            _manageFileService = manageFileService;
            ApiLocalUrl = _configuration["ApiUrlLocal"];
        }

        public DataSet GetAllBeneFactorData(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Params = new SqlParameter[5];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@ApiUrl", ApiLocalUrl);
            Params[2] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[3] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[4] = new SqlParameter("@IsFilter", false);
            var dt = _sQLHelper.ExecuteDataset("web.SP_GetAllBeneFactorsDataWithFilters", Params);
            return dt;
        }

        public List<FilterModel> GetAllBeneFactorFilters(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Params = new SqlParameter[5];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@ApiUrl", ApiLocalUrl);
            Params[2] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[3] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[4] = new SqlParameter("@IsFilter", true);
            var dt = _sQLHelper.ExecuteDataTable("web.SP_GetAllBeneFactorsDataWithFilters", Params);
            var Filters = _sQLHelper.GroupingFilters(dt);
            return Filters;
        }

        public async Task<HandleErrorResponseModel> AddNewBeneFactor(BeneFactors Model)
        {
            try
            {
                var Code = _sQLHelper.GenerateCode();
                var Response = new HandleErrorResponseModel();
                var BeneFactorObj = new BeneFactors();
                BeneFactorObj.Code = Code;
                BeneFactorObj.FullName = Model.FullName;
                BeneFactorObj.Description = Model.Description;
                BeneFactorObj.Phone = Model.Phone;
                BeneFactorObj.Phone2 = Model.Phone2;
                BeneFactorObj.Address = Model.Address;
                BeneFactorObj.Nationality = Model.Nationality;
                BeneFactorObj.FaceBook = Model.FaceBook;
                BeneFactorObj.InsertUser = Model.InsertUser;
                BeneFactorObj.InsertDate = DateTime.Now;

                var FileName = await _manageFileService.UploadFile(Model.Files, "", ImageFiles.BeneFactorImages);
                if (FileName.Done)
                    BeneFactorObj.Image = FileName.StringValue;
                else
                    return FileName;

                _Context.BeneFactors.Add(BeneFactorObj);
                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم اضافة متبرع جديد بنجاح";
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

        public async Task<HandleErrorResponseModel> UpdateBeneFactor(BeneFactors Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var BeneFactorObj = _Context.BeneFactors.FirstOrDefault(x => x.Id == Model.Id);
                BeneFactorObj.FullName = Model.FullName;
                BeneFactorObj.Description = Model.Description;
                BeneFactorObj.Phone = Model.Phone;
                BeneFactorObj.Phone2 = Model.Phone2;
                BeneFactorObj.Address = Model.Address;
                BeneFactorObj.Nationality = Model.Nationality;
                BeneFactorObj.FaceBook = Model.FaceBook;
                BeneFactorObj.UpdateUser = Model.InsertUser;
                BeneFactorObj.UpdateDate = DateTime.Now;

                if (Model.Files != null)
                {
                    var FileName = await _manageFileService.UploadFile(Model.Files, Model.OldFileName, ImageFiles.BeneFactorImages);
                    if (FileName.Done)
                        BeneFactorObj.Image = FileName.StringValue;
                    else
                        return FileName;
                }

                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم تعديل المتبرع بنجاح";
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

        public HandleErrorResponseModel DeleteBeneFactor(int BeneFactorId)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var BeneFactor = _Context.BeneFactors.FirstOrDefault(i => i.Id == BeneFactorId);
                if (BeneFactor != null)
                {
                    var File = _manageFileService.DeleteFile(BeneFactor.Image, ImageFiles.BeneFactorImages);
                    if (File.Done)
                    {
                        _Context.BeneFactors.Remove(BeneFactor);
                        _Context.SaveChanges();
                        Response.Done = true;
                        Response.Message = "تم حذف المتبرع بنجاح";
                        return Response;
                    }
                    else
                    {
                        Response.Done = false;
                        Response.Message = "لقد حدث خطا";
                        return Response;
                    }
                }
                else
                {
                    Response.Done = false;
                    Response.Message = "هذا المتبرع غير موجود";
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
    }
}
