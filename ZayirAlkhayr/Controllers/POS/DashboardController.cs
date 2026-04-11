using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.POS;

namespace ZayirAlkhayr.Controllers.POS
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("GetDashboardStatistics")]
        public async Task<ApiResponseModel<DataTable>> GetDashboardStatistics()
        {
            var results = await _dashboardService.GetDashboardStatistics();
            return results;
        }

        [HttpGet("GetTop3Orders")]
        public async Task<ApiResponseModel<List<Order>>> GetTop3Orders()
        {
            var results = await _dashboardService.GetTop3Orders();
            return results;
        }

        [HttpGet("GetTopSellingItemsToday")]
        public async Task<ApiResponseModel<DataTable>> GetTopSellingItemsToday()
        {
            var results = await _dashboardService.GetTopSellingItemsToday();
            return results;
        }

        [HttpGet("GetTodayOrdersStats")]
        public async Task<ApiResponseModel<object>> GetTodayOrdersStats()
        {
            var results = await _dashboardService.GetTodayOrdersStats();
            return results;
        }

        [HttpGet("GetCustomerDeliveryInsights")]
        public async Task<ApiResponseModel<DataTable>> GetCustomerDeliveryInsights()
        {
            var results = await _dashboardService.GetCustomerDeliveryInsights();
            return results;
        }
    }
}
