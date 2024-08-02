using Microsoft.Data.SqlClient;
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
    public class AccountsMonyService: IAccountsMonyService
    {
        private readonly ZADbContext _Context;
        private readonly ISQLHelper _sQLHelper;
        public AccountsMonyService(ZADbContext context, ISQLHelper sQLHelper)
        {
            _Context = context;
            _sQLHelper = sQLHelper;
        }

        public DataTable GetAllAccountsExportMony(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Date = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.ItemId;
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@Date", Date);
            Params[2] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[3] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllAccountsExportMony", Params);
            return dt;
        }

        public DataTable GetAllAccountsImportMony(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Date = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.ItemId;
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@Date", Date);
            Params[2] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[3] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllAccountsImportMony", Params);
            return dt;
        }

        public DataTable GetAllImportExportMonyStatistics(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Date = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.ItemId;
            var Params = new SqlParameter[2];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@Date", Date);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllImportExportMonyStatistics", Params);
            return dt;
        }

        public HandleErrorResponseModel AddNewAccountsImportMony(AccountsImportMony Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var ImportObj = new AccountsImportMony();
                ImportObj.TotalValue = Model.TotalValue;
                ImportObj.Details = Model.Details;
                ImportObj.InsertUser = Model.InsertUser;
                ImportObj.InsertDate = DateTime.Now.AddHours(1);

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

        public HandleErrorResponseModel AddNewAccountsExportMony(AccountsExportMony Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var ExportObj = new AccountsExportMony();
                ExportObj.TotalValue = Model.TotalValue;
                ExportObj.Details = Model.Details;
                ExportObj.InsertUser = Model.InsertUser;
                ExportObj.InsertDate = DateTime.Now.AddHours(1);

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
    }
}
