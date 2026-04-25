using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Reports;
using ZayirAlkhayr.Reports.Configuration;
using ZayirAlkhayr.Reports.Interface;
using ZayirAlkhayr.Reports.Model;
using ZayirAlkhayr.Reports.Service;

namespace ZayirAlkhayr.Reports.PdfTemplate
{
    public class DailySalesPdfReportGenerator : ReportGeneratorBase
    {
        private readonly IReportTemplateRenderer _reportTemplateRenderer;
        private readonly IPDFHelper _pdfHelper;
        private readonly IDailySalesReportDataSource _dailySalesReportDataSource;

        public DailySalesPdfReportGenerator(
            IReportTemplateRenderer reportTemplateRenderer,
            IPDFHelper pdfHelper,
            IDailySalesReportDataSource dailySalesReportDataSource,
            IOptions<ReportOptions> options,
            ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            _reportTemplateRenderer = reportTemplateRenderer;
            _pdfHelper = pdfHelper;
            _dailySalesReportDataSource = dailySalesReportDataSource;
        }

        public override ReportType ReportType => ReportType.DailySalesReport;
        public override ExportFormat Format => ExportFormat.Pdf;

        public override async Task<ReportFileResult> GenerateAsync(SearchReportModel model, CancellationToken cancellationToken = default)
        {
            var context = BuildContext(model,reportDisplayName: "تقرير المبيعات اليومية",fileNamePrefix: "DailySalesReport",sheetName: "DailySales",templateKey: "DailySalesReport.cshtml");

            var templateModel = await _dailySalesReportDataSource.BuildPdfModelAsync(model, context, cancellationToken);
            var html = await _reportTemplateRenderer.RenderAsync(context.TemplateKey, templateModel, cancellationToken);
            return await _pdfHelper.SaveHtmlResultAsync(html, context, cancellationToken);
        }
    }
}
