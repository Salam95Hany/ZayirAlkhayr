using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Interface.Report;

namespace ZayirAlkhayr.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemReportController : ControllerBase
    {
        private readonly IItemReportService _itemReportService;
        public ItemReportController(IItemReportService itemReportService)
        {
            _itemReportService = itemReportService;
        }

        [HttpPost("GetReportAllItemsSummary")]
        public async Task<ApiResponseModel<DataTable>> GetReportAllItemsSummary(PagingFilterModel PagingFilter)
        {
            var results = await _itemReportService.GetReportAllItemsSummary(PagingFilter);
            return results;
        }

        [HttpPost("GetReportAllItemsData")]
        public async Task<ApiResponseModel<DataTable>> GetReportAllItemsData(PagingFilterModel PagingFilter)
        {
            var results = await _itemReportService.GetReportAllItemsData(PagingFilter);
            return results;
        }

        [HttpPost("GetReportNeverSoldItemsSummary")]
        public async Task<ApiResponseModel<DataTable>> GetReportNeverSoldItemsSummary(PagingFilterModel PagingFilter)
        {
            var results = await _itemReportService.GetReportNeverSoldItemsSummary(PagingFilter);
            return results;
        }

        [HttpPost("GetReportNeverSoldItemsData")]
        public async Task<ApiResponseModel<DataTable>> GetReportNeverSoldItemsData(PagingFilterModel PagingFilter)
        {
            var results = await _itemReportService.GetReportNeverSoldItemsData(PagingFilter);
            return results;
        }

        [HttpPost("GetReportTopSellingItemSummary")]
        public async Task<ApiResponseModel<DataTable>> GetReportTopSellingItemSummary(PagingFilterModel PagingFilter)
        {
            var results = await _itemReportService.GetReportTopSellingItemSummary(PagingFilter);
            return results;
        }

        [HttpPost("GetReportTopSellingItemsData")]
        public async Task<ApiResponseModel<DataTable>> GetReportTopSellingItemsData(PagingFilterModel PagingFilter)
        {
            var results = await _itemReportService.GetReportTopSellingItemsData(PagingFilter);
            return results;
        }

        [HttpPost("GetReportLowestSellingItemsSummary")]
        public async Task<ApiResponseModel<DataTable>> GetReportLowestSellingItemsSummary(PagingFilterModel PagingFilter)
        {
            var results = await _itemReportService.GetReportLowestSellingItemsSummary(PagingFilter);
            return results;
        }

        [HttpPost("GetReportLowestSellingItemsData")]
        public async Task<ApiResponseModel<DataTable>> GetReportLowestSellingItemsData(PagingFilterModel PagingFilter)
        {
            var results = await _itemReportService.GetReportLowestSellingItemsData(PagingFilter);
            return results;
        }
    }
}
