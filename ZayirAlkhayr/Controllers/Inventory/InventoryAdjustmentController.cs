using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
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

        [HttpPost("GetAllInventoryAdjustmentData")]
        public async Task<ApiResponseModel<DataTable>> GetAllInventoryAdjustmentData(PagingFilterModel model)
        {
            return await _inventoryAdjustmentService.GetAllInventoryAdjustmentData(model);
        }

        [HttpPost("GetAllInventoryAdjustmentFilters")]
        public async Task<ApiResponseModel<List<FilterModel>>> GetAllInventoryAdjustmentFilters(PagingFilterModel model)
        {
            return await _inventoryAdjustmentService.GetAllInventoryAdjustmentFilters(model);
        }

        [HttpGet("GetInventoryAdjustmentById")]
        public async Task<ApiResponseModel<InventoryAdjustmentDto>> GetInventoryAdjustmentById(int inventoryAdjustmentId)
        {
            return await _inventoryAdjustmentService.GetInventoryAdjustmentById(inventoryAdjustmentId);
        }

        [HttpGet("GetAdjustmentDetailsById")]
        public async Task<ApiResponseModel<List<InventoryAdjustmentDetailsModelDto>>> GetAdjustmentDetailsById(int inventoryAdjustmentId)
        {
            return await _inventoryAdjustmentService.GetAdjustmentDetailsById(inventoryAdjustmentId);
        }

        [HttpPost("AddInventoryAdjustment")]
        public async Task<ApiResponseModel<string>> AddInventoryAdjustment(InventoryAdjustmentRequest model)
        {
            return await _inventoryAdjustmentService.AddInventoryAdjustment(model);
        }

        [HttpPost("UpdateInventoryAdjustment")]
        public async Task<ApiResponseModel<string>> UpdateInventoryAdjustment(InventoryAdjustmentRequest model)
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
