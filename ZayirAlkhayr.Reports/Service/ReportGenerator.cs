using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Reports;
using ZayirAlkhayr.Reports.Configuration;
using ZayirAlkhayr.Reports.Interface;
using ZayirAlkhayr.Reports.Model;

namespace ZayirAlkhayr.Reports.Service
{
    public abstract class ReportGeneratorBase : IReportGenerator
    {
        private readonly ReportOptions _options;
        protected readonly ILogger Logger;

        protected ReportGeneratorBase(IOptions<ReportOptions> options, ILoggerFactory loggerFactory)
        {
            _options = options?.Value ?? new ReportOptions();
            Logger = loggerFactory.CreateLogger(GetType());
        }

        public abstract ReportType ReportType { get; }
        public abstract ExportFormat Format { get; }
        public abstract Task<ReportFileResult> GenerateAsync(SearchReportModel model, CancellationToken cancellationToken = default);

        protected ReportRequestContext BuildContext(SearchReportModel request,string reportDisplayName,string fileNamePrefix,string sheetName,string templateKey = "")
        {
            var cultureName = !string.IsNullOrWhiteSpace(request.Culture)
                ? request.Culture
                : request.GetQueryValue("Culture");

            if (string.IsNullOrWhiteSpace(cultureName))
            {
                cultureName = _options.DefaultCulture;
            }

            CultureInfo culture;
            try
            {
                culture = CultureInfo.GetCultureInfo(cultureName);
            }
            catch (CultureNotFoundException)
            {
                Logger.LogWarning("Unknown report culture '{CultureName}', fallback to default culture '{DefaultCulture}'.",
                    cultureName,
                    _options.DefaultCulture);
                culture = CultureInfo.GetCultureInfo(_options.DefaultCulture);
            }

            var dateFormat = !string.IsNullOrWhiteSpace(request.DateFormat)
                ? request.DateFormat
                : request.GetQueryValue("DateFormat");

            if (string.IsNullOrWhiteSpace(dateFormat))
            {
                dateFormat = _options.DefaultDateFormat;
            }

            var resolvedFileNamePrefix = !string.IsNullOrWhiteSpace(request.FileNamePrefix)
                ? request.FileNamePrefix
                : fileNamePrefix;

            return new ReportRequestContext(
                ReportType,
                Format,
                culture,
                dateFormat,
                reportDisplayName,
                resolvedFileNamePrefix,
                sheetName,
                templateKey ?? string.Empty,
                string.IsNullOrWhiteSpace(request.UserName) ? "System" : request.UserName!,
                culture.TextInfo.IsRightToLeft,
                DateTimeOffset.Now);
        }
    }
}
