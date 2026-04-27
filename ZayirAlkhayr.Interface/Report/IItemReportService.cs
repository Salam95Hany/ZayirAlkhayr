using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;

namespace ZayirAlkhayr.Interface.Report
{
    public interface IItemReportService
    {
        Task<ApiResponseModel<DataTable>> GetReportAllItemsSummary(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetReportAllItemsData(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetExportReportAllItemsData(List<FilterModel> FilterList);
        Task<ApiResponseModel<DataTable>> GetReportNeverSoldItemsSummary(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetReportNeverSoldItemsData(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetExportReportNeverSoldItems(List<FilterModel> FilterList);
        Task<ApiResponseModel<DataTable>> GetReportTopSellingItemSummary(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetReportTopSellingItemsData(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetExportReportTopSellingItemsData(List<FilterModel> FilterList);
        Task<ApiResponseModel<DataTable>> GetReportLowestSellingItemsSummary(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetReportLowestSellingItemsData(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetExportReportLowestSellingItems(List<FilterModel> FilterList);
    }
}
