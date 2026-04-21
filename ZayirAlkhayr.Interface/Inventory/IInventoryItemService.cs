using System.Collections.Generic;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs.Inventory;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface.Inventory
{
    public interface IInventoryItemService
    {
        Task<ApiResponseModel<List<InventoryItemDetailsDto>>> GetAllInventoryItems(PagingFilterModel model);
        Task<ApiResponseModel<InventoryItemDetailsDto>> GetInventoryItemById(int inventoryItemId);
        Task<ApiResponseModel<List<InventoryItemDetailsDto>>> GetLowStockInventoryItems();
        Task<ApiResponseModel<string>> AddNewInventoryItem(InventoryItem model);
        Task<ApiResponseModel<string>> UpdateInventoryItem(InventoryItem model);
        Task<ApiResponseModel<string>> DeleteInventoryItem(int inventoryItemId);
    }
}
