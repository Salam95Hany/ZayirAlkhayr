using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _inventoryService;

        public PurchaseController(IPurchaseService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpPost("GetAllPurchases")]
        public async Task<ApiResponseModel<List<PurchaseSummaryDto>>> GetAllPurchases(PagingFilterModel model)
        {
            return await _inventoryService.GetAllPurchases(model);
        }

        [HttpGet("GetPurchaseById")]
        public async Task<ApiResponseModel<PurchaseDetailsDto>> GetPurchaseById(int purchaseId)
        {
            return await _inventoryService.GetPurchaseById(purchaseId);
        }

        [HttpPost("AddPurchase")]
        public async Task<ApiResponseModel<string>> AddPurchase(PurchaseUpsertDto model)
        {
            return await _inventoryService.AddPurchase(model);
        }

        [HttpPost("UpdatePurchase")]
        public async Task<ApiResponseModel<string>> UpdatePurchase(PurchaseUpsertDto model)
        {
            return await _inventoryService.UpdatePurchase(model);
        }

        [HttpGet("DeletePurchase")]
        public async Task<ApiResponseModel<string>> DeletePurchase(int purchaseId)
        {
            return await _inventoryService.DeletePurchase(purchaseId);
        }
    }
}
