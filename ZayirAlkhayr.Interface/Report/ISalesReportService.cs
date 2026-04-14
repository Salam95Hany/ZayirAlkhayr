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
        Task<ApiResponseModel<DataTable>> GetSalesReportStatistics(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetReportSalesData(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<List<FilterModel>>> GetReportSalesFilter(PagingFilterModel PagingFilter);
    }
}
