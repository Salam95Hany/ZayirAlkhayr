using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ZayirAlkhayr.Reports.Configuration;
using ZayirAlkhayr.Reports.Interface;
using ZayirAlkhayr.Reports.Model;

namespace ZayirAlkhayr.Reports.Service
{
    public class DailySalesJsonReportGenerator : DailySalesTabularReportGeneratorBase
    {
        public DailySalesJsonReportGenerator(
            IExportManagerService exportManagerService,
            IDailySalesReportDataSource dataSource,
            IOptions<ReportOptions> options,
            ILoggerFactory loggerFactory)
            : base(exportManagerService, dataSource, options, loggerFactory)
        {
        }

        public override ExportFormat Format => ExportFormat.Json;
    }
}
