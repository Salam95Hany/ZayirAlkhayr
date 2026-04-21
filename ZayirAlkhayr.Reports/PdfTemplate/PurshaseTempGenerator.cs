using Microsoft.AspNetCore.Hosting;
using RazorLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Reports;
using ZayirAlkhayr.Reports.Interface;
using ZayirAlkhayr.Reports.Model;
using ZayirAlkhayr.Reports.Service;

namespace ZayirAlkhayr.Reports.PdfTemplate
{
    public class PurshaseTempGenerator: ReportGenerator, IReportGenerator
    {
        private readonly IWebHostEnvironment _environment;
        public override ReportType ReportType => ReportType.DailySalesReport;

        public PurshaseTempGenerator(IWebHostEnvironment environment, IRazorLightEngine razorEngine, IPDFHelper pDFHelper) : base(razorEngine, pDFHelper)
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
                //var Results = await _reportsDataService.GetAdmissionTempData(int.Parse(PatientId), int.Parse(AdmissionId), int.Parse(SurgicalId));
                var Data = new PdfDataReports
                {
                    Data = new System.Data.DataTable(),
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
