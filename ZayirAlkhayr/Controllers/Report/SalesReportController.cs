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

        [HttpPost("GetSalesReportStatistics")]
        public async Task<ApiResponseModel<DataTable>> GetSalesReportStatistics(PagingFilterModel PagingFilter)
        {
            var results = await _salesReportService.GetSalesReportStatistics(PagingFilter);
            return results;
        }

        [HttpPost("GetReportSalesData")]
        public async Task<ApiResponseModel<DataTable>> GetReportSalesData(PagingFilterModel PagingFilter)
        {
            var results = await _salesReportService.GetReportSalesData(PagingFilter);
            return results;
        }

        [HttpPost("GetReportSalesFilter")]
        public async Task<ApiResponseModel<List<FilterModel>>> GetReportSalesFilter(PagingFilterModel PagingFilter)
        {
            var results = await _salesReportService.GetReportSalesFilter(PagingFilter);
            return results;
        }
    }
}
