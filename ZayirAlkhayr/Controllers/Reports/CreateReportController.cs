using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Reports;
using ZayirAlkhayr.Reports.Interface;
using ZayirAlkhayr.Reports.Model;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Controllers.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateReportController : ControllerBase
    {
        private readonly IReportGeneratorFactory _factory;

        public CreateReportController(IReportGeneratorFactory factory)
        {
            _factory = factory;
        }

        [HttpPost("CreateGeneralReport")]
        public async Task<IActionResult> CreateGeneralReport([FromBody] SearchReportModel model, CancellationToken cancellationToken)
        {
            if (model == null)
            {
                return BadRequest("بيانات التقرير مطلوبة.");
            }

            if (!Enum.TryParse<ReportType>(model.ReportType, true, out var reportType))
            {
                return BadRequest("نوع التقرير غير صحيح.");
            }

            var outputFormat = !string.IsNullOrWhiteSpace(model.OutputFormat)
                ? model.OutputFormat
                : model.GetQueryValue("Format");

            if (!Enum.TryParse<ExportFormat>(outputFormat, true, out var exportFormat))
            {
                exportFormat = ExportFormat.Excel;
            }

            try
            {
                var generator = _factory.GetGenerator(reportType, exportFormat);
                var generatedFile = await generator.GenerateAsync(model, cancellationToken);

                if (generatedFile == null || string.IsNullOrWhiteSpace(generatedFile.FilePath) || !System.IO.File.Exists(generatedFile.FilePath))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "فشل إنشاء ملف التقرير.");
                }

                return new TempPhysicalFileResult(generatedFile.FilePath, generatedFile.ContentType)
                {
                    FileDownloadName = string.IsNullOrWhiteSpace(generatedFile.DownloadFileName)
                        ? Path.GetFileName(generatedFile.FilePath)
                        : generatedFile.DownloadFileName
                };
            }
            catch (NotSupportedException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "حدث خطأ أثناء إنشاء التقرير.");
            }
        }
    }
}
