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
        Task<ApiResponseModel<DataTable>> GetExportDailySalesDetailsData(List<FilterModel> FilterList);
        Task<ApiResponseModel<DataTable>> GetReportMonthlySalesSummary(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetReportMonthlySalesDetailsData(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<List<FilterModel>>> GetReportMonthlySalesDetailsFilter(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetExportMonthlySalesDetailsData(List<FilterModel> FilterList);

    }
}
