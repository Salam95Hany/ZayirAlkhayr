using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RazorLight;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Reports;
using ZayirAlkhayr.Reports.Interface;
using ZayirAlkhayr.Reports.Model;
using ZayirAlkhayr.Reports.Service;

namespace ZayirAlkhayr.Reports.PdfTemplate
{
    public class DailySalesPdfReportGenerator : ReportGenerator
    {
        private readonly IWebHostEnvironment _environment;
        public override ReportType ReportType => ReportType.PurchasePDFReport;

        public DailySalesPdfReportGenerator(IWebHostEnvironment environment, IRazorLightEngine razorEngine, IPDFHelper pDFHelper) : base(razorEngine, pDFHelper)
        {
            _environment = environment;
        }


        public async Task<string> Generate(SearchReportModel Model)
        {
            var PatientId = Model.QueryString.FirstOrDefault(i => i.Key == "PatientId")?.Value;
            var AdmissionId = Model.QueryString.FirstOrDefault(i => i.Key == "AdmissionId")?.Value;
            var SurgicalId = Model.QueryString.FirstOrDefault(i => i.Key == "SurgicalId")?.Value;
            if (!string.IsNullOrEmpty(PatientId) && !string.IsNullOrEmpty(AdmissionId) && !string.IsNullOrEmpty(SurgicalId))
            {
                var Results = new DataTable();
                var Data = new PdfDataReports
                {
                    Data = Results,
                    ImageSrc = Path.Combine(_environment.WebRootPath, "Template", "Logo.png")
                };

                var FullPath = await this.Build(Data);
                return FullPath;
            }
            else
                return string.Empty;

        }
    }
}
