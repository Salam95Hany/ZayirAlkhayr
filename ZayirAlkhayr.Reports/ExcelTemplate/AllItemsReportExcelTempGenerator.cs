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
    public class AllItemsReportExcelTempGenerator: IReportGenerator
    {
        private readonly IExportManagerService _exportManagerService;
        private readonly IItemReportService _itemReportService;
        public AllItemsReportExcelTempGenerator(IExportManagerService exportManagerService, IItemReportService itemReportService)
        {
            _exportManagerService = exportManagerService;
            _itemReportService = itemReportService;
        }

        public ReportType ReportType => ReportType.AllItemsReport;

        public async Task<string> Generate(SearchReportModel Model)
        {
            var Data = await _itemReportService.GetExportReportAllItemsData(Model.FilterList);
            var ExportTemplate = new ExportTemplateBase { Name = "كل العناصر", SheetName = "كل العناصر", TemplateName = "كل العناصر", UserName = Model.UserName };
            var File = _exportManagerService.Export(ExportTemplate, Data.Results);
            return File;
        }
    }
}
