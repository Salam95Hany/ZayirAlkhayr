using System.Threading;
using System.Threading.Tasks;
using ZayirAlkhayr.Reports.Model;

namespace ZayirAlkhayr.Reports.Interface
{
    public interface IPDFHelper
    {
        Task<ReportFileResult> SaveHtmlResultAsync(string html, ReportRequestContext context, CancellationToken cancellationToken = default);
    }
}
