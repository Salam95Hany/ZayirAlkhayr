using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Reports;
using ZayirAlkhayr.Reports.Configuration;
using ZayirAlkhayr.Reports.Interface;
using ZayirAlkhayr.Reports.Model;

namespace ZayirAlkhayr.Reports.Service
{
    public abstract class DailySalesTabularReportGeneratorBase : ReportGeneratorBase
    {
        private readonly IExportManagerService _exportManagerService;
        private readonly IDailySalesReportDataSource _dailySalesReportDataSource;

        protected DailySalesTabularReportGeneratorBase(
            IExportManagerService exportManagerService,
            IDailySalesReportDataSource dailySalesReportDataSource,
            IOptions<ReportOptions> options,
            ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            _exportManagerService = exportManagerService;
            _dailySalesReportDataSource = dailySalesReportDataSource;
        }

        public override ReportType ReportType => ReportType.DailySalesReport;

        public override Task<ReportFileResult> GenerateAsync(SearchReportModel model, CancellationToken cancellationToken = default)
        {
            var context = BuildContext(
                model,
                reportDisplayName: "تقرير المبيعات اليومية",
                fileNamePrefix: "DailySalesReport",
                sheetName: "DailySales");

            return _exportManagerService.ExportAsync(context,_dailySalesReportDataSource.ReadDetailsBatchesAsync(model, cancellationToken),cancellationToken);
        }
    }
}
