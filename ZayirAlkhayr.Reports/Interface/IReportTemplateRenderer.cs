using System.Threading;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Reports.Interface
{
    public interface IReportTemplateRenderer
    {
        Task<string> RenderAsync(string templateKey, object model, CancellationToken cancellationToken = default);
    }
}
