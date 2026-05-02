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
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpPost("GetAllSuppliers")]
        public async Task<ApiResponseModel<List<SupplierSummaryDto>>> GetAllSuppliers(PagingFilterModel model)
        {
            return await _supplierService.GetAllSuppliers(model);
        }

        [HttpGet("GetSupplierById")]
        public async Task<ApiResponseModel<Supplier>> GetSupplierById(int supplierId)
        {
            return await _supplierService.GetSupplierById(supplierId);
        }

        [HttpPost("AddNewSupplier")]
        public async Task<ApiResponseModel<string>> AddNewSupplier(Supplier model)
        {
            return await _supplierService.AddNewSupplier(model);
        }

        [HttpPost("UpdateSupplier")]
        public async Task<ApiResponseModel<string>> UpdateSupplier(Supplier model)
        {
            return await _supplierService.UpdateSupplier(model);
        }

        [HttpGet("DeleteSupplier")]
        public async Task<ApiResponseModel<string>> DeleteSupplier(int supplierId)
        {
            return await _supplierService.DeleteSupplier(supplierId);
        }
    }
}
