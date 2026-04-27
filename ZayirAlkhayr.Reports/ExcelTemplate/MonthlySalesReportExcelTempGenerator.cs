using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Reports;
using ZayirAlkhayr.Interface.Report;
using ZayirAlkhayr.Reports.Interface;
using ZayirAlkhayr.Reports.Model;

namespace ZayirAlkhayr.Reports.ExcelTemplate
{
    public class MonthlySalesReportExcelTempGenerator: IReportGenerator
    {
        private readonly IExportManagerService _exportManagerService;
        private readonly ISalesReportService _salesReportService;
        public MonthlySalesReportExcelTempGenerator(IExportManagerService exportManagerService, ISalesReportService salesReportService)
        {
            _exportManagerService = exportManagerService;
            _salesReportService = salesReportService;
        }

        public ReportType ReportType => ReportType.MonthlySalesReport;

        public async Task<string> Generate(SearchReportModel Model)
        {
            var Data = await _salesReportService.GetExportMonthlySalesDetailsData(Model.FilterList);
            var ExportTemplate = new ExportTemplateBase { Name = "المبيعات الشهرية", SheetName = "المبيعات الشهرية", TemplateName = "المبيعات الشهرية", UserName = Model.UserName };
            var File = _exportManagerService.Export(ExportTemplate, Data.Results);
            return File;
        }
    }
}
