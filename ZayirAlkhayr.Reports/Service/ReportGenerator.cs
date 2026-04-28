using RazorLight;
using System;
using System.Threading.Tasks;
using ZayirAlkhayr.Reports.Interface;
using ZayirAlkhayr.Reports.Model;

namespace ZayirAlkhayr.Reports.Service
{
    public abstract class ReportGenerator
    {
        public abstract ReportType ReportType { get; }
        private readonly IRazorLightEngine _razorEngine;
        private readonly IPDFHelper _pDFHelper;

        protected ReportGenerator(IRazorLightEngine razorEngine, IPDFHelper pDFHelper)
        {
            _razorEngine = razorEngine;
            _pDFHelper = pDFHelper;
        }

        public async Task<string> Build(object Model)
        {
            try
            {
                var html = await _razorEngine.CompileRenderAsync(ReportType.ToString() + ".cshtml", Model);
                var FilePath = _pDFHelper.SaveHTMLResult(html);
                return FilePath;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
