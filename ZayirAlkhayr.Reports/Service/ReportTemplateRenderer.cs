using Microsoft.Extensions.Logging;
using RazorLight;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZayirAlkhayr.Reports.Interface;

namespace ZayirAlkhayr.Reports.Service
{
    public class ReportTemplateRenderer : IReportTemplateRenderer
    {
        private readonly IRazorLightEngine _razorLightEngine;
        private readonly ILogger<ReportTemplateRenderer> _logger;

        public ReportTemplateRenderer(IRazorLightEngine razorLightEngine, ILogger<ReportTemplateRenderer> logger)
        {
            _razorLightEngine = razorLightEngine;
            _logger = logger;
        }

        public async Task<string> RenderAsync(string templateKey, object model, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(templateKey))
            {
                throw new ArgumentException("A template key is required.", nameof(templateKey));
            }

            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                return await _razorLightEngine.CompileRenderAsync(templateKey, model);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to render report template '{TemplateKey}'.", templateKey);
                throw;
            }
        }
    }
}
