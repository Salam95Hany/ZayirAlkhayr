using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs.Inventory;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface.Inventory
{
    public interface IInventoryAdjustmentService
    {
        Task<ApiResponseModel<DataTable>> GetAllInventoryAdjustmentData(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<List<FilterModel>>> GetAllInventoryAdjustmentFilters(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<InventoryAdjustmentDto>> GetInventoryAdjustmentById(int inventoryAdjustmentId);
        Task<ApiResponseModel<List<InventoryAdjustmentDetailsModelDto>>> GetAdjustmentDetailsById(int inventoryAdjustmentId);
        Task<ApiResponseModel<string>> AddInventoryAdjustment(InventoryAdjustmentRequest model);
        Task<ApiResponseModel<string>> UpdateInventoryAdjustment(InventoryAdjustmentRequest model);
        Task<ApiResponseModel<string>> DeleteInventoryAdjustment(int inventoryAdjustmentId);
    }
}
