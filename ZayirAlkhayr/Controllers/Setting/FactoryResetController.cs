using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs.Setting;
using ZayirAlkhayr.Interface.Setting;

namespace ZayirAlkhayr.Controllers.Setting
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FactoryResetController : ControllerBase
    {
        private readonly IFactoryResetService _factoryResetService;

        public FactoryResetController(IFactoryResetService factoryResetService)
        {
            _factoryResetService = factoryResetService;
        }

        [HttpGet("GetFactoryResetPreview")]
        public async Task<ApiResponseModel<List<FactoryResetPreviewDto>>> GetFactoryResetPreview()
        {
            return await _factoryResetService.GetFactoryResetPreviewAsync();
        }

        [HttpPost("DeleteTargetData")]
        public async Task<ApiResponseModel<FactoryResetStepResultDto>> DeleteTargetData([FromBody] FactoryResetTargetRequestDto request)
        {
            return await _factoryResetService.DeleteTargetDataAsync(request?.TargetKey);
        }
    }
}
