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
    public class TopSellingItemReportExcelTempGenerator: IReportGenerator
    {
        private readonly IExportManagerService _exportManagerService;
        private readonly IItemReportService _itemReportService;
        public TopSellingItemReportExcelTempGenerator(IExportManagerService exportManagerService, IItemReportService itemReportService)
        {
            _exportManagerService = exportManagerService;
            _itemReportService = itemReportService;
        }

        public ReportType ReportType => ReportType.TopSellingItemsReport;

        public async Task<string> Generate(SearchReportModel Model)
        {
            var Data = await _itemReportService.GetExportReportTopSellingItemsData(Model.FilterList);
            var ExportTemplate = new ExportTemplateBase { Name = "الأكثر مبيعا", SheetName = "الأكثر مبيعا", TemplateName = "الأكثر مبيعا", UserName = Model.UserName };
            var File = _exportManagerService.Export(ExportTemplate, Data.Results);
            return File;
        }
    }
}
