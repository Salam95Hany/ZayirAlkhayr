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
        Task<ApiResponseModel<DataTable>> GetReportNeverSoldItemsSummary(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetReportNeverSoldItemsData(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetReportTopSellingItemSummary(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetReportTopSellingItemsData(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetReportLowestSellingItemsSummary(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetReportLowestSellingItemsData(PagingFilterModel PagingFilter);
    }
}
