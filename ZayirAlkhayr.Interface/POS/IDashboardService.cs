using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface.POS
{
    public interface IDashboardService
    {
        Task<ApiResponseModel<DataTable>> GetDashboardStatistics();
        Task<ApiResponseModel<List<Order>>> GetTop3Orders();
        Task<ApiResponseModel<DataTable>> GetTopSellingItemsToday();
        Task<ApiResponseModel<object>> GetTodayOrdersStats();
        Task<ApiResponseModel<DataTable>> GetCustomerDeliveryInsights();
    }
}
