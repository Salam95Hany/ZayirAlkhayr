using System.Collections.Generic;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs.Inventory;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface.Inventory
{
    public interface IInventoryAdjustmentService
    {
        Task<ApiResponseModel<List<InventoryAdjustmentDetailsDto>>> GetAllInventoryAdjustments(PagingFilterModel model);
        Task<ApiResponseModel<InventoryAdjustmentDetailsDto>> GetInventoryAdjustmentById(int inventoryAdjustmentId);
        Task<ApiResponseModel<string>> AddInventoryAdjustment(InventoryAdjustment model);
        Task<ApiResponseModel<string>> UpdateInventoryAdjustment(InventoryAdjustment model);
        Task<ApiResponseModel<string>> DeleteInventoryAdjustment(int inventoryAdjustmentId);
    }
}
