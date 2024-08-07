using Microsoft.Data.SqlClient;
using PosSystem.Entities.Common;
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
    public class AccountsMonyService : IAccountsMonyService
    {
        private readonly ZADbContext _Context;
        private readonly ISQLHelper _sQLHelper;
        private readonly IExportManagerService _exportManagerService;
        public AccountsMonyService(ZADbContext context, ISQLHelper sQLHelper, IExportManagerService exportManagerService)
        {
            _Context = context;
            _sQLHelper = sQLHelper;
            _exportManagerService = exportManagerService;
        }

        public DataTable GetAllAccountsExportMonyData(PagingFilterModel PagingFilter)
        {
            int MonthNum = 0;
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Date = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.ItemId;
            var Month = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Month")?.ItemId;
            if (!string.IsNullOrEmpty(Month))
                MonthNum = DateTime.Parse(Month).Month;
            var Params = new SqlParameter[6];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@Date", Date);
            Params[2] = new SqlParameter("@Month", MonthNum);
            Params[3] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[4] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[5] = new SqlParameter("@IsFilter", false);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllAccountsExportMonyDataWithFilter", Params);
            return dt;
        }

        public List<FilterModel> GetAllAccountsExportMonyFilters(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Date = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.ItemId;
            var Params = new SqlParameter[5];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@Date", Date);
            Params[2] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[3] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[4] = new SqlParameter("@IsFilter", true);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllAccountsExportMonyDataWithFilter", Params);
            var Filters = _sQLHelper.GroupingFilters(dt);
            return Filters;
        }

        public DataTable GetAllAccountsImportMonyData(PagingFilterModel PagingFilter)
        {
            int MonthNum = 0;
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Date = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.ItemId;
            var Month = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Month")?.ItemId;
            if (!string.IsNullOrEmpty(Month))
                MonthNum = DateTime.Parse(Month).Month;
            var Params = new SqlParameter[6];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@Date", Date);
            Params[2] = new SqlParameter("@Month", MonthNum);
            Params[3] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[4] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[5] = new SqlParameter("@IsFilter", false);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllAccountsImportMonyDataWithFilter", Params);
            return dt;
        }

        public List<FilterModel> GetAllAccountsImportMonyFilters(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Date = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.ItemId;
            var Params = new SqlParameter[5];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@Date", Date);
            Params[2] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[3] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[4] = new SqlParameter("@IsFilter", true);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllAccountsImportMonyDataWithFilter", Params);
            var Filters = _sQLHelper.GroupingFilters(dt);
            return Filters;
        }

        public DataTable GetAllImportExportMonyStatistics(PagingFilterModel PagingFilter)
        {
            int MonthNum = 0;
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Date = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.ItemId;
            var Month = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Month")?.ItemId;
            if (!string.IsNullOrEmpty(Month))
                MonthNum = DateTime.Parse(Month).Month;
            var Params = new SqlParameter[3];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@Date", Date);
            Params[2] = new SqlParameter("@Month", MonthNum);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllImportExportMonyStatistics", Params);
            return dt;
        }

        public DataSet ExportAccountsImportMonyData(PDFModel Model)
        {
            int MonthNum = 0;
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(Model.FilterList);
            var Date = Model.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.ItemId;
            var Month = Model.FilterList.FirstOrDefault(i => i.CategoryName == "Month")?.ItemId;
            if (!string.IsNullOrEmpty(Month))
                MonthNum = DateTime.Parse(Month).Month;

            var Params = new SqlParameter[3];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@Date", Date);
            Params[2] = new SqlParameter("@Month", MonthNum);
            var dt = _sQLHelper.ExecuteDataset("admin.SP_ExportAccountsImportMonyData", Params);
            return dt;
        }

        public DataSet ExportAccountsExportMonyData(PDFModel Model)
        {
            int MonthNum = 0;
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(Model.FilterList);
            var Date = Model.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.ItemId;
            var Month = Model.FilterList.FirstOrDefault(i => i.CategoryName == "Month")?.ItemId;
            if (!string.IsNullOrEmpty(Month))
                MonthNum = DateTime.Parse(Month).Month;
            var Params = new SqlParameter[3];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@Date", Date);
            Params[2] = new SqlParameter("@Month", MonthNum);
            var dt = _sQLHelper.ExecuteDataset("admin.SP_ExportAccountsExportMonyData", Params);
            return dt;
        }

        public string ExportAccountsImportMonyExcelFile(PDFModel Model, string UserName)
        {
            var Dt = ExportAccountsImportMonyData(Model);
            Model.Headers = Dt.Tables[1].AsEnumerable().Select(i => new PDFHeaderSelected
            {
                NameEn = i.Field<string>("DisplayValue"),
                NameAr = i.Field<string>("DisplayName")
            }).ToList();

            var ExportTemplate = new ExportTemplateBase { Name = "الايرادات", SheetName = "الايرادات", TemplateName = "الايرادات", UserName = UserName, Header = new ExportHeaders { ListHeaders = Model.Headers } };
            var File = _exportManagerService.Export(ExportTemplate, Dt.Tables[0]);
            return File;
        }

        public string ExportAccountsExportMonyExcelFile(PDFModel Model, string UserName)
        {
            var Dt = ExportAccountsExportMonyData(Model);
            Model.Headers = Dt.Tables[1].AsEnumerable().Select(i => new PDFHeaderSelected
            {
                NameEn = i.Field<string>("DisplayValue"),
                NameAr = i.Field<string>("DisplayName")
            }).ToList();

            var ExportTemplate = new ExportTemplateBase { Name = "الصادرات", SheetName = "الصادرات", TemplateName = "الصادرات", UserName = UserName, Header = new ExportHeaders { ListHeaders = Model.Headers } };
            var File = _exportManagerService.Export(ExportTemplate, Dt.Tables[0]);
            return File;
        }

        public HandleErrorResponseModel AddNewAccountsImportMony(AccountsImportMony Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var ImportObj = new AccountsImportMony();
                ImportObj.BeneFactorId = Model.BeneFactorId;
                ImportObj.BeneFactorTypeId = Model.BeneFactorTypeId;
                ImportObj.TotalValue = Model.TotalValue;
                ImportObj.Details = Model.Details;
                ImportObj.InsertUser = Model.InsertUser;
                ImportObj.InsertDate = Model.InsertDate;

                _Context.AccountsImportMony.Add(ImportObj);
                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم اضافة مبلغ جديد بنجاح";
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

        public HandleErrorResponseModel UpdateAccountsImportMony(AccountsImportMony Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var ImportObj = _Context.AccountsImportMony.FirstOrDefault(x => x.Id == Model.Id);
                ImportObj.BeneFactorId = Model.BeneFactorId;
                ImportObj.BeneFactorTypeId = Model.BeneFactorTypeId;
                ImportObj.TotalValue = Model.TotalValue;
                ImportObj.Details = Model.Details;
                ImportObj.InsertDate = Model.InsertDate;
                ImportObj.UpdateUser = Model.InsertUser;
                ImportObj.UpdateDate = DateTime.Now.AddHours(1);

                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم تعديل الايراد بنجاح";
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

        public HandleErrorResponseModel DeleteAccountsImportMony(int AccountId)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var ImportObj = _Context.AccountsImportMony.FirstOrDefault(i => i.Id == AccountId);
                _Context.AccountsImportMony.Remove(ImportObj);
                _Context.SaveChanges();
                Response.Done = true;
                Response.Message = "تم حذف الايراد بنجاح";
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

        public HandleErrorResponseModel AddNewAccountsExportMony(AccountsExportMony Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var ExportObj = new AccountsExportMony();
                ExportObj.BeneFactorId = Model.BeneFactorId;
                ExportObj.BeneFactorTypeId = Model.BeneFactorTypeId;
                ExportObj.TotalValue = Model.TotalValue;
                ExportObj.Details = Model.Details;
                ExportObj.InsertUser = Model.InsertUser;
                ExportObj.InsertDate = Model.InsertDate;

                _Context.AccountsExportMony.Add(ExportObj);
                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم اضافة مبلغ جديد بنجاح";
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

        public HandleErrorResponseModel UpdateAccountsExportMony(AccountsExportMony Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var ImportObj = _Context.AccountsExportMony.FirstOrDefault(x => x.Id == Model.Id);
                ImportObj.BeneFactorId = Model.BeneFactorId;
                ImportObj.BeneFactorTypeId = Model.BeneFactorTypeId;
                ImportObj.TotalValue = Model.TotalValue;
                ImportObj.Details = Model.Details;
                ImportObj.InsertDate = Model.InsertDate;
                ImportObj.UpdateUser = Model.InsertUser;
                ImportObj.UpdateDate = DateTime.Now.AddHours(1);

                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم تعديل الصادر بنجاح";
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

        public HandleErrorResponseModel DeleteAccountsExportMony(int AccountId)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var ExportObj = _Context.AccountsExportMony.FirstOrDefault(i => i.Id == AccountId);
                _Context.AccountsExportMony.Remove(ExportObj);
                _Context.SaveChanges();
                Response.Done = true;
                Response.Message = "تم حذف الصادر بنجاح";
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
    }
}
