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

        #region SalesReport
        public async Task<ApiResponseModel<DataTable>> GetSalesReportStatistics(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[3];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@FromDate", FromDate);
            Params[2] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_GetSalesReportStatistics]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<DataTable>> GetReportSalesData(PagingFilterModel PagingFilter)
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
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_GetReportSalesDataWithFilter]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<List<FilterModel>>> GetReportSalesFilter(PagingFilterModel PagingFilter)
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
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_GetReportSalesDataWithFilter]", Params);
            var Filters = dt.ToGroupedFilters();
            return ApiResponseModel<List<FilterModel>>.Success(GenericErrors.GetSuccess, Filters);
        }
        #endregion

        #region ItemSalesReport
        public async Task<ApiResponseModel<DataSet>> GetReportSalesItemStatistics(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[3];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@FromDate", FromDate);
            Params[2] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDatasetAsync("[Report].[SP_GetItemSalesReportStatistics]", Params);
            return ApiResponseModel<DataSet>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<DataTable>> GetReportSalesItemData(PagingFilterModel PagingFilter)
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
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_GetReportSalesItemDataWithFilter]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<List<FilterModel>>> GetReportSalesItemFilter(PagingFilterModel PagingFilter)
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
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Report].[SP_GetReportSalesItemDataWithFilter]", Params);
            var Filters = dt.ToGroupedFilters();
            return ApiResponseModel<List<FilterModel>>.Success(GenericErrors.GetSuccess, Filters);
        }
        #endregion

        #region OrderTypeSalesReport
        public async Task<ApiResponseModel<DataSet>> GetOrderTypeSalesReport(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var Params = new SqlParameter[2];
            Params[0] = new SqlParameter("@FromDate", FromDate);
            Params[1] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDatasetAsync("[Report].[SP_GetOrderTypeSalesReport]", Params);
            return ApiResponseModel<DataSet>.Success(GenericErrors.GetSuccess, dt);
        }
        #endregion

        #region CustomerSalesReport
        public async Task<ApiResponseModel<DataSet>> GetCustomerSalesReport(PagingFilterModel PagingFilter)
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
            var dt = await _sQLHelper.ExecuteDatasetAsync("[Report].[SP_GetCustomerSalesReport]", Params);
            return ApiResponseModel<DataSet>.Success(GenericErrors.GetSuccess, dt);
        }
        #endregion

        #region SalesByTimeReport
        public async Task<ApiResponseModel<DataSet>> GetSalesReportByTimeReport(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[1] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[2] = new SqlParameter("@FromDate", FromDate);
            Params[3] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDatasetAsync("[Report].[SP_GetSalesReportByTimeReport]", Params);
            return ApiResponseModel<DataSet>.Success(GenericErrors.GetSuccess, dt);
        }
        #endregion

    }
}
