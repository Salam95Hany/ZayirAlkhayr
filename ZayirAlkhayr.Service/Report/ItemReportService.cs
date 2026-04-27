using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Interface.Common;
using ZayirAlkhayr.Interface.Report;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service.Report
{
    public class ItemReportService : IItemReportService
    {
        private readonly ISQLHelper _sQLHelper;
        public ItemReportService(ISQLHelper sQLHelper)
        {
            _sQLHelper = sQLHelper;
        }

        #region AllItemsReport
        public async Task<ApiResponseModel<DataTable>> GetReportAllItemsSummary(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[3];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@FromDate", FromDate);
            Params[2] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_ReportAllItemsSummary]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<DataTable>> GetReportAllItemsData(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[5];
            Params[0] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[1] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[2] = new SqlParameter("@FilterList", FilterDt);
            Params[3] = new SqlParameter("@FromDate", FromDate);
            Params[4] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_ReportAllItems]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<DataTable>> GetExportReportAllItemsData(List<FilterModel> FilterList)
        {
            var FromDate = FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[3];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@FromDate", FromDate);
            Params[2] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_GetExportReportAllItemsData]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }
        #endregion

        #region NeverSoldItemsReport
        public async Task<ApiResponseModel<DataTable>> GetReportNeverSoldItemsSummary(PagingFilterModel PagingFilter)
        {
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[1];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_ReportNeverSoldItemsSummary]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<DataTable>> GetReportNeverSoldItemsData(PagingFilterModel PagingFilter)
        {
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[3];
            Params[0] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[1] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[2] = new SqlParameter("@FilterList", FilterDt);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_ReportNeverSoldItems]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<DataTable>> GetExportReportNeverSoldItems(List<FilterModel> FilterList)
        {
            var FilterDt = FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[1];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_ExportReportNeverSoldItems]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }
        #endregion

        #region TopSellingItemsReport
        public async Task<ApiResponseModel<DataTable>> GetReportTopSellingItemSummary(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var Params = new SqlParameter[2];
            Params[0] = new SqlParameter("@FromDate", FromDate);
            Params[1] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_ReportTopSellingItemSummary]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<DataTable>> GetReportTopSellingItemsData(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var Params = new SqlParameter[2];
            Params[0] = new SqlParameter("@FromDate", FromDate);
            Params[1] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_ReportTopSellingItems]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<DataTable>> GetExportReportTopSellingItemsData(List<FilterModel> FilterList)
        {
            var FromDate = FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[2];
            Params[0] = new SqlParameter("@FromDate", FromDate);
            Params[1] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_ExportReportTopSellingItemsData]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }
        #endregion

        #region LowestSellingItemsReport
        public async Task<ApiResponseModel<DataTable>> GetReportLowestSellingItemsSummary(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var Params = new SqlParameter[2];
            Params[0] = new SqlParameter("@FromDate", FromDate);
            Params[1] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_ReportLowestSellingItemsSummary]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<DataTable>> GetReportLowestSellingItemsData(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var Params = new SqlParameter[2];
            Params[0] = new SqlParameter("@FromDate", FromDate);
            Params[1] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_ReportLowestSellingItems]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<DataTable>> GetExportReportLowestSellingItems(List<FilterModel> FilterList)
        {
            var FromDate = FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[2];
            Params[0] = new SqlParameter("@FromDate", FromDate);
            Params[1] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_ExportReportLowestSellingItems]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }
        #endregion
    }
}
