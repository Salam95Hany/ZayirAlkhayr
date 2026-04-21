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
    public class InventoryAdjustmentController : ControllerBase
    {
        private readonly IInventoryAdjustmentService _inventoryAdjustmentService;

        public InventoryAdjustmentController(IInventoryAdjustmentService inventoryAdjustmentService)
        {
            _inventoryAdjustmentService = inventoryAdjustmentService;
        }

        [HttpPost("GetAllInventoryAdjustments")]
        public async Task<ApiResponseModel<List<InventoryAdjustmentDetailsDto>>> GetAllInventoryAdjustments(PagingFilterModel model)
        {
            return await _inventoryAdjustmentService.GetAllInventoryAdjustments(model);
        }

        [HttpGet("GetInventoryAdjustmentById")]
        public async Task<ApiResponseModel<InventoryAdjustmentDetailsDto>> GetInventoryAdjustmentById(int inventoryAdjustmentId)
        {
            return await _inventoryAdjustmentService.GetInventoryAdjustmentById(inventoryAdjustmentId);
        }

        [HttpPost("AddInventoryAdjustment")]
        public async Task<ApiResponseModel<string>> AddInventoryAdjustment(InventoryAdjustment model)
        {
            return await _inventoryAdjustmentService.AddInventoryAdjustment(model);
        }

        [HttpPost("UpdateInventoryAdjustment")]
        public async Task<ApiResponseModel<string>> UpdateInventoryAdjustment(InventoryAdjustment model)
        {
            return await _inventoryAdjustmentService.UpdateInventoryAdjustment(model);
        }

        [HttpGet("DeleteInventoryAdjustment")]
        public async Task<ApiResponseModel<string>> DeleteInventoryAdjustment(int inventoryAdjustmentId)
        {
            return await _inventoryAdjustmentService.DeleteInventoryAdjustment(inventoryAdjustmentId);
        }
    }
}
