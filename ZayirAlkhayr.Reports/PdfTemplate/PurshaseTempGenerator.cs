using Microsoft.AspNetCore.Hosting;
using RazorLight;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Reports;
using ZayirAlkhayr.Interface.Inventory;
using ZayirAlkhayr.Reports.Interface;
using ZayirAlkhayr.Reports.Model;
using ZayirAlkhayr.Reports.Service;

namespace ZayirAlkhayr.Reports.PdfTemplate
{
    public class DailySalesPdfReportGenerator : ReportGenerator, IReportGenerator
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IPurchaseService _purchaseService;
        public override ReportType ReportType => ReportType.PurchasePDFReport;

        public DailySalesPdfReportGenerator(IWebHostEnvironment environment, IPDFQuestHelper QuestHelper, IPurchaseService purchaseService) : base(QuestHelper)
        {
            _environment = environment;
            _purchaseService = purchaseService;
        }


        public async Task<string> Generate(SearchReportModel Model)
        {
            var PurchaseId = Model.QueryString.FirstOrDefault(i => i.Key == "PurchaseId")?.Value;
            if (!string.IsNullOrEmpty(PurchaseId))
            {
                var Results = await _purchaseService.GetPurchaseById(int.Parse(PurchaseId));
                var Data = new PurchaseData();

                Data.PurchaseNumber = Results.Results.PurchaseNumber;
                Data.SupplierName = Results.Results.SupplierName;
                Data.SupplierPhone = Results.Results.SupplierPhone;
                Data.TotalAmount = Results.Results.TotalAmount;
                Data.InsertDate = Results.Results.InsertDate;
                Data.Items = Results.Results.Items;
                Data.ImageSrc = Path.Combine(_environment.WebRootPath, "Template", "Dams_Star.png");
                Data.HandleData();
                var FullPath = await this.Build(Data);
                return FullPath;
            }
            else
                return string.Empty;

        }
    }
}
