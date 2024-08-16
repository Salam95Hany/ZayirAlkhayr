using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.GeneralServices;

namespace ZayirAlkhayr.Controllers.GeneralServices
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyNeedsController : ControllerBase
    {
        private readonly IFamilyNeedsService _familyNeedsService;
        public FamilyNeedsController(IFamilyNeedsService familyNeedsService)
        {
            _familyNeedsService = familyNeedsService;
        }

        [HttpPost("GetAllFamilyNeedTypesData")]
        public DataTable GetAllFamilyNeedTypesData(PagingFilterModel PagingFilter)
        {
            var results = _familyNeedsService.GetAllFamilyNeedTypesData(PagingFilter);
            return results;
        }

        [HttpPost("GetAllFamilyNeedTypesFilters")]
        public List<FilterModel> GetAllFamilyNeedTypesFilters(PagingFilterModel PagingFilter)
        {
            var results = _familyNeedsService.GetAllFamilyNeedTypesFilters(PagingFilter);
            return results;
        }

        [HttpPost("GetAllFamilyNeedCategoriesData")]
        public DataTable GetAllFamilyNeedCategoriesData(PagingFilterModel PagingFilter)
        {
            var results = _familyNeedsService.GetAllFamilyNeedCategoriesData(PagingFilter);
            return results;
        }

        [HttpPost("GetAllFamilyNeedCategoriesFilters")]
        public List<FilterModel> GetAllFamilyNeedCategoriesFilters(PagingFilterModel PagingFilter)
        {
            var results = _familyNeedsService.GetAllFamilyNeedCategoriesFilters(PagingFilter);
            return results;
        }

        [HttpGet("GetAllFamilyNeedCategories")]
        public List<FamilyNeedCategories> GetAllFamilyNeedCategories()
        {
            var results = _familyNeedsService.GetAllFamilyNeedCategories();
            return results;
        }

        [HttpPost("AddNewFamilyNeedType")]
        public HandleErrorResponseModel AddNewFamilyNeedType(FamilyNeedTypes Model)
        {
            var results = _familyNeedsService.AddNewFamilyNeedType(Model);
            return results;
        }

        [HttpPost("AddNewFamilyNeedCategory")]
        public HandleErrorResponseModel AddNewFamilyNeedCategory(FamilyNeedCategories Model)
        {
            var results = _familyNeedsService.AddNewFamilyNeedCategory(Model);
            return results;
        }

        [HttpPost("UpdateFamilyNeedType")]
        public HandleErrorResponseModel UpdateFamilyNeedType(FamilyNeedTypes Model)
        {
            var results = _familyNeedsService.UpdateFamilyNeedType(Model);
            return results;
        }

        [HttpPost("UpdateFamilyNeedCategory")]
        public HandleErrorResponseModel UpdateFamilyNeedCategory(FamilyNeedCategories Model)
        {
            var results = _familyNeedsService.UpdateFamilyNeedCategory(Model);
            return results;
        }

        [HttpGet("DeleteFamilyNeedType")]
        public HandleErrorResponseModel DeleteFamilyNeedType(int NeedTypeId)
        {
            var results = _familyNeedsService.DeleteFamilyNeedType(NeedTypeId);
            return results;
        }

        [HttpGet("DeleteFamilyNeedCategory")]
        public HandleErrorResponseModel DeleteFamilyNeedCategory(int CategoryId)
        {
            var results = _familyNeedsService.DeleteFamilyNeedCategory(CategoryId);
            return results;
        }
    }
}
