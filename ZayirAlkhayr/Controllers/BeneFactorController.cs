using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface;

namespace ZayirAlkhayr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneFactorController : ControllerBase
    {
        private readonly IBeneFactorService _beneFactorService;
        public BeneFactorController(IBeneFactorService beneFactorService)
        {
            _beneFactorService = beneFactorService;
        }

        [HttpPost("GetAllBeneFactorData")]
        public DataSet GetAllBeneFactorData(PagingFilterModel PagingFilter)
        {
            var results = _beneFactorService.GetAllBeneFactorData(PagingFilter);
            return results;
        }

        [HttpPost("GetAllBeneFactorFilters")]
        public List<FilterModel> GetAllBeneFactorFilters(PagingFilterModel PagingFilter)
        {
            var results = _beneFactorService.GetAllBeneFactorFilters(PagingFilter);
            return results;
        }

        [HttpPost("AddNewBeneFactor")]
        public async Task<HandleErrorResponseModel> AddNewBeneFactor(BeneFactors Model)
        {
            var results = await _beneFactorService.AddNewBeneFactor(Model);
            return results;
        }

        [HttpPost("UpdateBeneFactor")]
        public async Task<HandleErrorResponseModel> UpdateBeneFactor(BeneFactors Model)
        {
            var results = await _beneFactorService.UpdateBeneFactor(Model);
            return results;
        }

        [HttpGet("DeleteBeneFactor")]
        public HandleErrorResponseModel DeleteBeneFactor(int BeneFactorId)
        {
            var results = _beneFactorService.DeleteBeneFactor(BeneFactorId);
            return results;
        }
    }
}
