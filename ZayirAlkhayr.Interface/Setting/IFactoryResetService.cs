using System.Collections.Generic;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs.Setting;

namespace ZayirAlkhayr.Interface.Setting
{
    public interface IFactoryResetService
    {
        Task<ApiResponseModel<List<FactoryResetPreviewDto>>> GetFactoryResetPreviewAsync();
        Task<ApiResponseModel<FactoryResetStepResultDto>> DeleteTargetDataAsync(string targetKey);
    }
}
