using System.Threading;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Reports;
using ZayirAlkhayr.Reports.Model;

namespace ZayirAlkhayr.Reports.Interface
{
    public interface IReportGenerator
    {
        ReportType ReportType { get; }
        ExportFormat Format { get; }
        Task<ReportFileResult> GenerateAsync(SearchReportModel model, CancellationToken cancellationToken = default);
    }
}
