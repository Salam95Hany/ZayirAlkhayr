using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;

namespace ZayirAlkhayr.Interface.Report
{
    public interface ISalesReportService
    {
        Task<ApiResponseModel<DataTable>> GetReportDailySalesSummary(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> ReportDailySalesDetailsData(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<List<FilterModel>>> ReportDailySalesDetailsFilter(PagingFilterModel PagingFilter);

        Task<ApiResponseModel<DataSet>> GetReportSalesItemStatistics(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetReportSalesItemData(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<List<FilterModel>>> GetReportSalesItemFilter(PagingFilterModel PagingFilter);

        Task<ApiResponseModel<DataSet>> GetOrderTypeSalesReport(PagingFilterModel PagingFilter);

        Task<ApiResponseModel<DataSet>> GetCustomerSalesReport(PagingFilterModel PagingFilter);

        Task<ApiResponseModel<DataSet>> GetSalesReportByTimeReport(PagingFilterModel PagingFilter);
    }
}
