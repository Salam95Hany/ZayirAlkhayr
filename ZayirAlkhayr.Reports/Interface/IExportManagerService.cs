using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZayirAlkhayr.Reports.Model;

namespace ZayirAlkhayr.Reports.Interface
{
    public interface IExportManagerService
    {
        Task<ReportFileResult> ExportAsync(
            ReportRequestContext context,
            IAsyncEnumerable<ReportDataBatch> dataBatches,
            CancellationToken cancellationToken = default);
    }
}
