using System.Collections.Generic;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface.Inventory
{
    public interface IUnitService
    {
        Task<ApiResponseModel<List<Unit>>> GetAllUnits(PagingFilterModel model);
        Task<ApiResponseModel<Unit>> GetUnitById(int unitId);
        Task<ApiResponseModel<string>> AddNewUnit(Unit model);
        Task<ApiResponseModel<string>> UpdateUnit(Unit model);
        Task<ApiResponseModel<string>> DeleteUnit(int unitId);
    }
}
