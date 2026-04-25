using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Reports;
using ZayirAlkhayr.Reports.Model;

namespace ZayirAlkhayr.Reports.Interface
{
    public interface IDailySalesReportDataSource
    {
        IAsyncEnumerable<ReportDataBatch> ReadDetailsBatchesAsync(SearchReportModel request, CancellationToken cancellationToken = default);
        Task<DailySalesPdfModel> BuildPdfModelAsync(SearchReportModel request, ReportRequestContext context, CancellationToken cancellationToken = default);
    }
}
