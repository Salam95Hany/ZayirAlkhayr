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
    public class SalesReportService : ISalesReportService
    {
        private readonly ISQLHelper _sQLHelper;
        public SalesReportService(ISQLHelper sQLHelper)
        {
            _sQLHelper = sQLHelper;
        }

        #region DailySalesReport
        public async Task<ApiResponseModel<DataTable>> GetReportDailySalesSummary(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[3];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@FromDate", FromDate);
            Params[2] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].SP_GetReportDailySalesSummary", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<DataTable>> ReportDailySalesDetailsData(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[6];
            Params[0] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[1] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[2] = new SqlParameter("@IsFilter", false);
            Params[3] = new SqlParameter("@FilterList", FilterDt);
            Params[4] = new SqlParameter("@FromDate", FromDate);
            Params[5] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_ReportDailySalesDetails]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<List<FilterModel>>> ReportDailySalesDetailsFilter(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[6];
            Params[0] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[1] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[2] = new SqlParameter("@IsFilter", true);
            Params[3] = new SqlParameter("@FilterList", FilterDt);
            Params[4] = new SqlParameter("@FromDate", FromDate);
            Params[5] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_ReportDailySalesDetails]", Params);
            var Filters = dt.ToGroupedFilters();
            return ApiResponseModel<List<FilterModel>>.Success(GenericErrors.GetSuccess, Filters);
        }
        #endregion

        #region MonthlySalesReport
        public async Task<ApiResponseModel<DataTable>> GetReportMonthlySalesSummary(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[3];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@FromDate", FromDate);
            Params[2] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].SP_ReportMonthlySalesSummary", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<DataTable>> GetReportMonthlySalesDetailsData(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[6];
            Params[0] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[1] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[2] = new SqlParameter("@IsFilter", false);
            Params[3] = new SqlParameter("@FilterList", FilterDt);
            Params[4] = new SqlParameter("@FromDate", FromDate);
            Params[5] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_ReportMonthlySalesDetails]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<List<FilterModel>>> GetReportMonthlySalesDetailsFilter(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[6];
            Params[0] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[1] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[2] = new SqlParameter("@IsFilter", true);
            Params[3] = new SqlParameter("@FilterList", FilterDt);
            Params[4] = new SqlParameter("@FromDate", FromDate);
            Params[5] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_ReportMonthlySalesDetails]", Params);
            var Filters = dt.ToGroupedFilters();
            return ApiResponseModel<List<FilterModel>>.Success(GenericErrors.GetSuccess, Filters);
        }
        #endregion

    }
}
