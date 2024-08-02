using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PosSystem.Entities.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
        private readonly ICreatePdfFileService _createPdfFileService;
        private readonly IExportManagerService _exportManagerService;
        private string ApiLocalUrl;
        public BeneFactorService(ZADbContext context, ISQLHelper sQLHelper, IConfiguration configuration, IManageFileService manageFileService, ICreatePdfFileService createPdfFileService, IExportManagerService exportManagerService)
        {
            _Context = context;
            _sQLHelper = sQLHelper;
            _configuration = configuration;
            _manageFileService = manageFileService;
            _createPdfFileService = createPdfFileService;
            _exportManagerService = exportManagerService;
            ApiLocalUrl = _configuration["ApiUrlLocal"];
        }

        public BeneFactorLoginModel BeneFactorLogin(int Code, string BeneFactorName)
        {
            var Response = new BeneFactorLoginModel();
            var result = _Context.BeneFactors.FirstOrDefault(i => i.Code == Code && i.FullName == BeneFactorName);
            var WelcomeMessage = _Context.BeneFactorWelcomeMessage.FirstOrDefault();
            if (result != null)
            {
                Response.BeneFactorId = result.Id;
                Response.Name = result.FullName;
                Response.Code = result.Code;
                Response.LoginId = Guid.NewGuid().ToString();
                Response.LoginDate = DateTime.Now.AddHours(1);
                Response.WelcomeMessage = WelcomeMessage == null ? "" : WelcomeMessage.Message;
                Response.ResponseCode = 200;
                Response.ResponseMessage = "تم تسجيل الدخول بنجاح";
                return Response;
            }
            else
            {
                Response.ResponseCode = 100;
                Response.ResponseMessage = "اسم المستخدم او الكود غير صالح";
                return Response;
            }

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

        public DataTable GetAllBeneFactorTypes(PagingFilterModel PagingFilter)
        {
            var SearchText = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "SearchText");
            var Params = new SqlParameter[3];
            Params[0] = new SqlParameter("@SearchText", SearchText?.ItemId);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            var dt = _sQLHelper.ExecuteDataTable("web.SP_GetAllBeneFactorTypes", Params);
            return dt;
        }

        public List<BeneFactorDetails> GetAllBeneFactorParentById(int BeneFactorId)
        {
            var results = _Context.BeneFactorDetails.Where(i => i.BeneFactorId == BeneFactorId && i.IsParent.Value).Select(i => new BeneFactorDetails
            {
                Id = i.Id,
                TotalValue = i.TotalValue,
                PaymentDateStr = i.PaymentDate.ToString("dddd d MMMM , yyyy", new CultureInfo("ar-AE")),
                IsActive = i.IsActive,
            }).ToList();
            return results;
        }

        public DataTable GetAllBeneFactorDetails(PagingFilterModel PagingFilter, int BeneFactorId)
        {
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@ApiUrl", ApiLocalUrl);
            Params[1] = new SqlParameter("@BeneFactorId", BeneFactorId);
            Params[2] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[3] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            var dt = _sQLHelper.ExecuteDataTable("web.SP_GetAllBeneFactorDetails", Params);
            return dt;
        }

        public DataTable GetAllBeneFactorCashDetails(int BeneFactorId, int ParentId)
        {
            var Params = new SqlParameter[3];
            Params[0] = new SqlParameter("@ApiUrl", ApiLocalUrl);
            Params[1] = new SqlParameter("@BeneFactorId", BeneFactorId);
            Params[2] = new SqlParameter("@ParentId", ParentId);
            var dt = _sQLHelper.ExecuteDataTable("web.SP_GetAllBeneFactorCashDetails", Params);
            return dt;
        }

        public DataTable GetBeneFactorDetailsByBeneFactorId(int BeneFactorId, int BeneFactorTypeId)
        {
            var Params = new SqlParameter[3];
            Params[0] = new SqlParameter("@ApiUrl", ApiLocalUrl);
            Params[1] = new SqlParameter("@BeneFactorId", BeneFactorId);
            Params[2] = new SqlParameter("@BeneFactorTypeId", BeneFactorTypeId);
            var dt = _sQLHelper.ExecuteDataTable("web.SP_GetBeneFactorDetailsByBeneFactorId", Params);
            return dt;
        }

        public DataTable GetBeneFactorDetailsStatistics(int BeneFactorId)
        {
            var Params = new SqlParameter[1];
            Params[0] = new SqlParameter("@BeneFactorId", BeneFactorId);
            var dt = _sQLHelper.ExecuteDataTable("web.SP_GetBeneFactorDetailsStatistics", Params);
            return dt;
        }

        public DataTable GetBeneFactorNotes(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Params = new SqlParameter[3];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            var dt = _sQLHelper.ExecuteDataTable("web.SP_GetBeneFactorNotes", Params);
            return dt;
        }

        public DataTable GetAllBeneFactorNationalities(PagingFilterModel PagingFilter)
        {
            var SearchText = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "SearchText");
            var Params = new SqlParameter[3];
            Params[0] = new SqlParameter("@SearchText", SearchText?.ItemId);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            var dt = _sQLHelper.ExecuteDataTable("web.SP_GetAllBeneFactorNationalities", Params);
            return dt;
        }

        public List<BeneFactorTypes> GetBeneFactorTypeByIds(List<int> Ids)
        {
            var results = _Context.BeneFactorTypes.Where(i => Ids.Contains(i.Id)).ToList();
            return results;
        }

        public BeneFactorWelcomeMessage GetBeneFactorWelcomeMessage()
        {
            var results = _Context.BeneFactorWelcomeMessage.FirstOrDefault();
            return results;
        }

        public DataTable ExportBeneFactorsData(PDFModel Model)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(Model.FilterList);
            var Params = new SqlParameter[1];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            var dt = _sQLHelper.ExecuteDataTable("web.SP_ExportBeneFactorsData", Params);
            return dt;
        }

        public string ExportBeneFactorsPDFFile(PDFModel Model, int RowCount)
        {
            var Dt = ExportBeneFactorsData(Model);
            for (int i = 0; i < Dt.Rows.Count; i++)
                Dt.Rows[i]["Id"] = (i + 1).ToString();

            var DtBatches = Dt.ToDataTableBatches(RowCount);
            Model.Headers.Add(new PDFHeaderSelected { DisplayOrder = 0, NameAr = "الرقم", NameEn = "Id", ValueType = "Text", IsAllowSummation = false });
            Model.Headers = Model.Headers.OrderBy(i => i.DisplayOrder).ToList();
            var File = _createPdfFileService.CreatePdfFile(DtBatches, Model.Headers, ImageFiles.ExportFiles.ToString(), "المتبرعين");
            return File;
        }

        public string ExportBeneFactorsExcelFile(PDFModel Model, string UserName)
        {
            var Dt = ExportBeneFactorsData(Model);
            for (int i = 0; i < Dt.Rows.Count; i++)
                Dt.Rows[i]["Id"] = (i + 1).ToString();

            Model.Headers.Add(new PDFHeaderSelected { DisplayOrder = 0, NameAr = "الرقم", NameEn = "Id", ValueType = "Text", IsAllowSummation = false });
            var ExportTemplate = new ExportTemplateBase { Name = "المتبرعين", SheetName = "المتبرعين", TemplateName = "المتبرعين", UserName = UserName, Header = new ExportHeaders { ListHeaders = Model.Headers } };
            var File = _exportManagerService.Export(ExportTemplate, Dt);
            return File;
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
                BeneFactorObj.NationalityId = Model.NationalityId;
                BeneFactorObj.FaceBook = Model.FaceBook;
                BeneFactorObj.InsertUser = Model.InsertUser;
                BeneFactorObj.InsertDate = DateTime.Now.AddHours(1);

                if (Model.Files != null)
                {
                    var FileName = await _manageFileService.UploadFile(Model.Files, "", ImageFiles.BeneFactorImages);
                    if (FileName.Done)
                        BeneFactorObj.Image = FileName.StringValue;
                    else
                        return FileName;
                }

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

        public HandleErrorResponseModel AddNewBeneFactorType(BeneFactorTypes Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var BeneFactorObj = new BeneFactorTypes();
                BeneFactorObj.Name = Model.Name;
                BeneFactorObj.InsertUser = Model.InsertUser;
                BeneFactorObj.InsertDate = DateTime.Now.AddHours(1);

                _Context.BeneFactorTypes.Add(BeneFactorObj);
                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم اضافة نوع جديد بنجاح";
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

        public HandleErrorResponseModel AddNewBeneFactorNationality(BeneFactorNationalities Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var BeneFactorObj = new BeneFactorNationalities();
                BeneFactorObj.Name = Model.Name;
                BeneFactorObj.InsertUser = Model.InsertUser;
                BeneFactorObj.InsertDate = DateTime.Now.AddHours(1);

                _Context.BeneFactorNationalities.Add(BeneFactorObj);
                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم اضافة جنسية جديدة بنجاح";
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

        public async Task<HandleErrorResponseModel> AddNewBeneFactorDetails(BeneFactorDetails Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var BeneFactorObj = new BeneFactorDetails();
                BeneFactorObj.BeneFactorId = Model.BeneFactorId;
                BeneFactorObj.ParentId = Model.ParentId;
                BeneFactorObj.BeneFactorTypeId = Model.BeneFactorTypeId;
                BeneFactorObj.Details = Model.Details;
                BeneFactorObj.PaymentDate = Model.PaymentDate;
                BeneFactorObj.TotalValue = Model.TotalValue;
                BeneFactorObj.InsertUser = Model.InsertUser;
                BeneFactorObj.InsertDate = DateTime.Now.AddHours(1);
                BeneFactorObj.IsParent = Model.IsParent;

                if (Model.Files != null)
                {
                    var FileName = await _manageFileService.UploadFile(Model.Files, "", ImageFiles.BeneFactorDetailsImages);
                    if (FileName.Done)
                        BeneFactorObj.Image = FileName.StringValue;
                    else
                        return FileName;
                }

                if (Model.IsFinalSubscribe)
                {
                    var BeneFactorParent = _Context.BeneFactorDetails.FirstOrDefault(i => i.Id == Model.ParentId);
                    if (BeneFactorParent != null)
                        BeneFactorParent.IsActive = true;
                }


                _Context.BeneFactorDetails.Add(BeneFactorObj);
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

        public HandleErrorResponseModel AddNewBeneFactorNotes(BeneFactorNotes Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var BeneFactorObj = new BeneFactorNotes();
                BeneFactorObj.BeneFactorId = Model.BeneFactorId;
                BeneFactorObj.Note = Model.Note;
                BeneFactorObj.Suggestion = Model.Suggestion;
                BeneFactorObj.InsertDate = DateTime.Now.AddHours(1);


                _Context.BeneFactorNotes.Add(BeneFactorObj);
                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم اضافة ملاحظاتك بنجاح";
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

        public HandleErrorResponseModel AddNewBeneFactorWelcomeMessage(BeneFactorWelcomeMessage Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var WelcomeMessage = _Context.BeneFactorWelcomeMessage.FirstOrDefault();
                if (WelcomeMessage == null)
                {
                    var BeneFactorObj = new BeneFactorWelcomeMessage();
                    BeneFactorObj.Message = Model.Message;
                    BeneFactorObj.InsertUser = Model.InsertUser;
                    BeneFactorObj.InsertDate = DateTime.Now.AddHours(1);

                    _Context.BeneFactorWelcomeMessage.Add(BeneFactorObj);
                }
                else
                {
                    WelcomeMessage.Message = Model.Message;
                    WelcomeMessage.UpdateDate = DateTime.Now.AddHours(1);
                    WelcomeMessage.UpdateUser = Model.UpdateUser;
                }

                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم اضافة رسالة ترحيبية بنجاح";
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
                BeneFactorObj.UpdateDate = DateTime.Now.AddHours(1);

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
                var BeneFactorDetails = _Context.BeneFactorDetails.Where(i => i.BeneFactorId == BeneFactorId).ToList();
                if (BeneFactor != null)
                {
                    if (BeneFactor.Image != null)
                        _manageFileService.DeleteFile(BeneFactor.Image, ImageFiles.BeneFactorImages);

                    _Context.BeneFactors.Remove(BeneFactor);
                    if (BeneFactorDetails.Count > 0)
                    {
                        foreach (var item in BeneFactorDetails)
                            if (item.Image != null)
                                _manageFileService.DeleteFile(item.Image, ImageFiles.BeneFactorDetailsImages);

                        _Context.BeneFactorDetails.RemoveRange(BeneFactorDetails);
                    }

                    _Context.SaveChanges();
                    Response.Done = true;
                    Response.Message = "تم حذف المتبرع بنجاح";
                    return Response;
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
