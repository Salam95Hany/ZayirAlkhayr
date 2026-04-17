using System.Collections.Generic;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface.Inventory
{
    public interface IInventoryItemService
    {
        Task<ApiResponseModel<List<InventoryItem>>> GetAllInventoryItems(PagingFilterModel model);
        Task<ApiResponseModel<InventoryItem>> GetInventoryItemById(int inventoryItemId);
        Task<ApiResponseModel<List<InventoryItem>>> GetLowStockInventoryItems();
        Task<ApiResponseModel<string>> AddNewInventoryItem(InventoryItem model);
        Task<ApiResponseModel<string>> UpdateInventoryItem(InventoryItem model);
        Task<ApiResponseModel<string>> DeleteInventoryItem(int inventoryItemId);
    }
}
