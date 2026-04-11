using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Entities.Specifications.Orders;
using ZayirAlkhayr.Interface.Common;
using ZayirAlkhayr.Interface.POS;
using ZayirAlkhayr.Interface.Repositories;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service.POS
{
    public class DashboardService: IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISQLHelper _sQLHelper;
        public DashboardService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper)
        {
            _unitOfWork = unitOfWork;
            _sQLHelper = sQLHelper;
        }

        public async Task<ApiResponseModel<DataTable>> GetDashboardStatistics()
        {
            var dt = await _sQLHelper.ExecuteDataTableAsync("[POS].[SP_OrderDashboardStatistics]", Array.Empty<SqlParameter>());
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<List<Order>>> GetTop3Orders()
        {
            var Spec = new Top3TodayOrdersSpecification(true);
            var Results = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(Spec);
            return ApiResponseModel<List<Order>>.Success(GenericErrors.GetSuccess, Results);
        }

        public async Task<ApiResponseModel<DataTable>> GetTopSellingItemsToday()
        {
            var dt = await _sQLHelper.ExecuteDataTableAsync("[POS].[SP_GetTopSellingItemsToday]", Array.Empty<SqlParameter>());
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<object>> GetTodayOrdersStats()
        {
            var Spec = new Top3TodayOrdersSpecification(false);
            var Results = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(Spec);

            var totalOrders = Results.Count();
            var completedOrders = Results.Where(o => o.OrderStatus == OrderStatus.Completed).Count();
            var cancelledOrders = Results.Where(o => o.OrderStatus == OrderStatus.Cancelled).Count();
            var completedPercent = totalOrders == 0 ? 0 : (completedOrders * 100.0 / totalOrders);
            var cancelledPercent = totalOrders == 0 ? 0 : (cancelledOrders * 100.0 / totalOrders);

            var result = new
            {
                CompletedOrders = completedOrders,
                CancelledOrders = cancelledOrders,
                CompletedPercent = Math.Round(completedPercent, 2),
                CancelledPercent = Math.Round(cancelledPercent, 2)
            };

            return ApiResponseModel<object>.Success(GenericErrors.GetSuccess, result);
        }

        public async Task<ApiResponseModel<DataTable>> GetCustomerDeliveryInsights()
        {
            var dt = await _sQLHelper.ExecuteDataTableAsync("[POS].[SP_CustomerDeliveryInsights]", Array.Empty<SqlParameter>());
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }
    }
}
