using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.GeneralServices;
using ZayirAlkhayr.Service.GeneralServices;

namespace ZayirAlkhayr.Controllers.GeneralServices
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyCategoryController : ControllerBase
    {
        private readonly IFamilyCategoryService _familyCategoryService;
        public FamilyCategoryController(IFamilyCategoryService familyCategoryService)
        {
            _familyCategoryService = familyCategoryService;
        }

        [HttpPost("GetAllFamilyCategoryData")]
        public DataTable GetAllFamilyCategoryData(PagingFilterModel PagingFilter)
        {
            var results = _familyCategoryService.GetAllFamilyCategoryData(PagingFilter);
            return results;
        }

        [HttpPost("GetAllFamilyCategoryFilter")]
        public List<FilterModel> GetAllFamilyCategoryFilter(PagingFilterModel PagingFilter)
        {
            var results = _familyCategoryService.GetAllFamilyCategoryFilter(PagingFilter);
            return results;
        }

        [HttpPost("AddNewFamilyCategory")]
        public HandleErrorResponseModel AddNewFamilyCategory(FamilyCategories Model)
        {
            var results = _familyCategoryService.AddNewFamilyCategory(Model);
            return results;
        }

        [HttpPost("UpdateFamilyCategory")]
        public HandleErrorResponseModel UpdateFamilyCategory(FamilyCategories Model)
        {
            var results = _familyCategoryService.UpdateFamilyCategory(Model);
            return results;
        }

        [HttpGet("DeleteFamilyCategory")]
        public HandleErrorResponseModel DeleteFamilyCategory(int CategoryId)
        {
            var results = _familyCategoryService.DeleteFamilyCategory(CategoryId);
            return results;
        }
    }
}
