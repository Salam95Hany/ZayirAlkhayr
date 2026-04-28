using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs.Inventory;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.Inventory;

namespace ZayirAlkhayr.Controllers.Inventory
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemController : ControllerBase
    {
        private readonly IInventoryItemService _inventoryItemService;

        public InventoryItemController(IInventoryItemService inventoryItemService)
        {
            _inventoryItemService = inventoryItemService;
        }

        [HttpPost("GetAllInventoryItems")]
        public async Task<ApiResponseModel<List<InventoryItemDetailsDto>>> GetAllInventoryItems(PagingFilterModel model)
        {
            return await _inventoryItemService.GetAllInventoryItems(model);
        }

        [HttpGet("GetInventoryItemById")]
        public async Task<ApiResponseModel<InventoryItemDetailsDto>> GetInventoryItemById(int inventoryItemId)
        {
            return await _inventoryItemService.GetInventoryItemById(inventoryItemId);
        }

        [HttpGet("GetLowStockInventoryItems")]
        public async Task<ApiResponseModel<List<InventoryItemDetailsDto>>> GetLowStockInventoryItems()
        {
            return await _inventoryItemService.GetLowStockInventoryItems();
        }

        [HttpPost("AddNewInventoryItem")]
        public async Task<ApiResponseModel<string>> AddNewInventoryItem(InventoryItem model)
        {
            return await _inventoryItemService.AddNewInventoryItem(model);
        }

        [HttpPost("UpdateInventoryItem")]
        public async Task<ApiResponseModel<string>> UpdateInventoryItem(InventoryItem model)
        {
            return await _inventoryItemService.UpdateInventoryItem(model);
        }

        [HttpGet("DeleteInventoryItem")]
        public async Task<ApiResponseModel<string>> DeleteInventoryItem(int inventoryItemId)
        {
            return await _inventoryItemService.DeleteInventoryItem(inventoryItemId);
        }
    }
}
