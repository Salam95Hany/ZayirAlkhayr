using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.Inventory;

namespace ZayirAlkhayr.Controllers.Inventory
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly IUnitService _unitService;

        public UnitController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        [HttpPost("GetAllUnits")]
        public async Task<ApiResponseModel<List<Unit>>> GetAllUnits(PagingFilterModel model)
        {
            return await _unitService.GetAllUnits(model);
        }

        [HttpGet("GetUnitById")]
        public async Task<ApiResponseModel<Unit>> GetUnitById(int unitId)
        {
            return await _unitService.GetUnitById(unitId);
        }

        [HttpPost("AddNewUnit")]
        public async Task<ApiResponseModel<string>> AddNewUnit(Unit model)
        {
            return await _unitService.AddNewUnit(model);
        }

        [HttpPost("UpdateUnit")]
        public async Task<ApiResponseModel<string>> UpdateUnit(Unit model)
        {
            return await _unitService.UpdateUnit(model);
        }

        [HttpGet("DeleteUnit")]
        public async Task<ApiResponseModel<string>> DeleteUnit(int unitId)
        {
            return await _unitService.DeleteUnit(unitId);
        }
    }
}
