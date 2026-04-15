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

        [HttpPost("GetReportSalesItemStatistics")]
        public async Task<ApiResponseModel<DataSet>> GetReportSalesItemStatistics(PagingFilterModel PagingFilter)
        {
            var results = await _salesReportService.GetReportSalesItemStatistics(PagingFilter);
            return results;
        }

        [HttpPost("GetReportSalesItemData")]
        public async Task<ApiResponseModel<DataTable>> GetReportSalesItemData(PagingFilterModel PagingFilter)
        {
            var results = await _salesReportService.GetReportSalesItemData(PagingFilter);
            return results;
        }

        [HttpPost("GetReportSalesItemFilter")]
        public async Task<ApiResponseModel<List<FilterModel>>> GetReportSalesItemFilter(PagingFilterModel PagingFilter)
        {
            var results = await _salesReportService.GetReportSalesItemFilter(PagingFilter);
            return results;
        }

        [HttpPost("GetOrderTypeSalesReport")]
        public async Task<ApiResponseModel<DataSet>> GetOrderTypeSalesReport(PagingFilterModel PagingFilter)
        {
            var results = await _salesReportService.GetOrderTypeSalesReport(PagingFilter);
            return results;
        }

        [HttpPost("GetCustomerSalesReport")]
        public async Task<ApiResponseModel<DataSet>> GetCustomerSalesReport(PagingFilterModel PagingFilter)
        {
            var results = await _salesReportService.GetCustomerSalesReport(PagingFilter);
            return results;
        }

        [HttpPost("GetSalesReportByTimeReport")]
        public async Task<ApiResponseModel<DataSet>> GetSalesReportByTimeReport(PagingFilterModel PagingFilter)
        {
            var results = await _salesReportService.GetSalesReportByTimeReport(PagingFilter);
            return results;
        }
    }
}
