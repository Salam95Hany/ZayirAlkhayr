using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ZayirAlkhayr.Reports.Configuration;
using ZayirAlkhayr.Reports.Interface;
using ZayirAlkhayr.Reports.Model;

namespace ZayirAlkhayr.Reports.Service
{
    public class DailySalesCsvReportGenerator : DailySalesTabularReportGeneratorBase
    {
        public DailySalesCsvReportGenerator(
            IExportManagerService exportManagerService,
            IDailySalesReportDataSource dataSource,
            IOptions<ReportOptions> options,
            ILoggerFactory loggerFactory)
            : base(exportManagerService, dataSource, options, loggerFactory)
        {
        }

        public override ExportFormat Format => ExportFormat.Csv;
    }
}
