using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Interface.Report;

namespace ZayirAlkhayr.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesReportController : ControllerBase
    {
        private readonly ISalesReportService _salesReportService;
        public SalesReportController(ISalesReportService salesReportService)
        {
            _salesReportService = salesReportService;
        }

        [HttpPost("GetReportDailySalesSummary")]
        public async Task<ApiResponseModel<DataTable>> GetReportDailySalesSummary(PagingFilterModel PagingFilter)
        {
            var results = await _salesReportService.GetReportDailySalesSummary(PagingFilter);
            return results;
        }

        [HttpPost("ReportDailySalesDetailsData")]
        public async Task<ApiResponseModel<DataTable>> ReportDailySalesDetailsData(PagingFilterModel PagingFilter)
        {
            var results = await _salesReportService.ReportDailySalesDetailsData(PagingFilter);
            return results;
        }

        [HttpPost("ReportDailySalesDetailsFilter")]
        public async Task<ApiResponseModel<List<FilterModel>>> ReportDailySalesDetailsFilter(PagingFilterModel PagingFilter)
        {
            var results = await _salesReportService.ReportDailySalesDetailsFilter(PagingFilter);
            return results;
        }

        [HttpPost("GetReportMonthlySalesSummary")]
        public async Task<ApiResponseModel<DataTable>> GetReportMonthlySalesSummary(PagingFilterModel PagingFilter)
        {
            var results = await _salesReportService.GetReportMonthlySalesSummary(PagingFilter);
            return results;
        }

        [HttpPost("GetReportMonthlySalesDetailsData")]
        public async Task<ApiResponseModel<DataTable>> GetReportMonthlySalesDetailsData(PagingFilterModel PagingFilter)
        {
            var results = await _salesReportService.GetReportMonthlySalesDetailsData(PagingFilter);
            return results;
        }

        [HttpPost("GetReportMonthlySalesDetailsFilter")]
        public async Task<ApiResponseModel<List<FilterModel>>> GetReportMonthlySalesDetailsFilter(PagingFilterModel PagingFilter)
        {
            var results = await _salesReportService.GetReportMonthlySalesDetailsFilter(PagingFilter);
            return results;
        }
    }
}
