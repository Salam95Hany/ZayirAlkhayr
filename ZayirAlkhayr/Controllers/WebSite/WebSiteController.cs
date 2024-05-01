using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PosSystem.Entities.Common;
using System;
using System.Data;
using ZayirAlkhayr.Interface.Admin;
using ZayirAlkhayr.Interface.WebSite;
using ZayirAlkhayr.Service.Admin;

namespace ZayirAlkhayr.Controllers.WebSite
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebSiteController : ControllerBase
    {
        private readonly IWebSiteService _webSiteService;
        private readonly IExportManagerService _exportManagerService;
        public WebSiteController(IWebSiteService webSiteService, IExportManagerService exportManagerService)
        {
            _webSiteService = webSiteService;
            _exportManagerService = exportManagerService;
        }

        [HttpGet("ExportFile")]
        public IActionResult ExportFile()
        {
            ExportTemplateBase exportTemplateBase = new ExportTemplateBase();

            exportTemplateBase.Name = "Test File";
            exportTemplateBase.TemplateName = "TestTestTest";
            exportTemplateBase.UserName = "سلام هاني";

            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Age", typeof(int));
            table.Columns.Add("Address", typeof(string));

            table.Rows.Add(1, "Salam Hany", 25, "Alex");
            table.Rows.Add(2, "Mostafa Gamal", 26, "Alex");
            table.Rows.Add(3, "Mahmoud Sayed", 27, "Alex");
            table.Rows.Add(4, "Ahmed Hamdy", 28, "Cairo");
            table.Rows.Add(5, "Said Arafa", 29, "Cairo");

            var filePath = _exportManagerService.Export(exportTemplateBase, table);
            return new TempPhysicalFileResult(filePath, "application/xlsx");

        }


    }
}
