using Microsoft.AspNetCore.Http;
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
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierPaymentController : ControllerBase
    {
        private readonly ISupplierPaymentService _supplierPaymentService;
        public SupplierPaymentController(ISupplierPaymentService supplierPaymentService)
        {
            _supplierPaymentService = supplierPaymentService;
        }

        [HttpPost("GetAllSupplierInvicesData")]
        public async Task<ApiResponseModel<DataTable>> GetAllSupplierInvicesData(PagingFilterModel PagingFilter)
        {
            var results = await _supplierPaymentService.GetAllSupplierInvicesData(PagingFilter);
            return results;
        }

        [HttpPost("GetAllSupplierInvicesFilters")]
        public async Task<ApiResponseModel<List<FilterModel>>> GetAllSupplierInvicesFilters(PagingFilterModel PagingFilter)
        {
            var results = await _supplierPaymentService.GetAllSupplierInvicesFilters(PagingFilter);
            return results;
        }

        [HttpPost("GetSupplierPaymentLogData")]
        public async Task<ApiResponseModel<DataTable>> GetSupplierPaymentLogData(PagingFilterModel PagingFilter)
        {
            var results = await _supplierPaymentService.GetSupplierPaymentLogData(PagingFilter);
            return results;
        }

        [HttpGet("GetSupplierNameById")]
        public async Task<ApiResponseModel<string>> GetSupplierNameById(int SupplierId)
        {
            var results = await _supplierPaymentService.GetSupplierNameById(SupplierId);
            return results;
        }

        [HttpGet("GetPurchaseNumberBySupplierId")]
        public async Task<ApiResponseModel<List<PurchaseNumbersDto>>> GetPurchaseNumberBySupplierId(int SupplierId)
        {
            var results = await _supplierPaymentService.GetPurchaseNumberBySupplierId(SupplierId);
            return results;
        }

        [HttpPost("AddNewSupplierPayment")]
        public async Task<ApiResponseModel<string>> AddNewSupplierPayment(SupplierPayment Model)
        {
            var results = await _supplierPaymentService.AddNewSupplierPayment(Model);
            return results;

        }

        [HttpPost("UpdateSupplierPayment")]
        public async Task<ApiResponseModel<string>> UpdateSupplierPayment(SupplierPayment Model)
        {
            var results = await _supplierPaymentService.UpdateSupplierPayment(Model);
            return results;

        }

        [HttpGet("DeleteSupplierPayment")]
        public async Task<ApiResponseModel<string>> DeleteSupplierPayment(int SupplierPaymentId)
        {
            var results = await _supplierPaymentService.DeleteSupplierPayment(SupplierPaymentId);
            return results;
        }
    }
}
