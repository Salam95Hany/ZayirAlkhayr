using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Reports;
using ZayirAlkhayr.Reports.Interface;
using ZayirAlkhayr.Reports.Model;

namespace ZayirAlkhayr.Reports.ExcelTemplate
{
    public class DailySalesReportExcelTempGenerator: IReportGenerator
    {
        private readonly IExportManagerService _exportManagerService;
        public DailySalesReportExcelTempGenerator(IExportManagerService exportManagerService)
        {
            _exportManagerService = exportManagerService;
        }

        public ReportType ReportType => ReportType.DailySalesReport;

        public async Task<string> Generate(SearchReportModel Model)
        {
            var ExportTemplate = new ExportTemplateBase { Name = "Patient Search", SheetName = "Patient Search", TemplateName = "Patient Search", UserName = Model.UserName };
            var File = _exportManagerService.Export(ExportTemplate, new DataTable());
            return File;
        }
    }
}
