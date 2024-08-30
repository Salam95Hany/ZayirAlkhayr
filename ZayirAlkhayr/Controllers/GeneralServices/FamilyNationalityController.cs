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
    public class FamilyNationalityController : ControllerBase
    {
        private readonly IFamilyNationalityService _familyNationalityService;
        public FamilyNationalityController(IFamilyNationalityService familyNationalityService)
        {
            _familyNationalityService = familyNationalityService;
        }

        [HttpPost("GetAllFamilyNationalitiesData")]
        public DataTable GetAllFamilyNationalitiesData(PagingFilterModel PagingFilter)
        {
            var results = _familyNationalityService.GetAllFamilyNationalitiesData(PagingFilter);
            return results;
        }

        [HttpPost("GetAllFamilyNationalitiesFilter")]
        public List<FilterModel> GetAllFamilyNationalitiesFilter(PagingFilterModel PagingFilter)
        {
            var results = _familyNationalityService.GetAllFamilyNationalitiesFilter(PagingFilter);
            return results;
        }

        [HttpPost("AddNewFamilyNationality")]
        public HandleErrorResponseModel AddNewFamilyNationality(FamilyNationalities Model)
        {
            var results = _familyNationalityService.AddNewFamilyNationality(Model);
            return results;
        }

        [HttpPost("UpdateFamilyNationality")]
        public HandleErrorResponseModel UpdateFamilyNationality(FamilyNationalities Model)
        {
            var results = _familyNationalityService.UpdateFamilyNationality(Model);
            return results;
        }

        [HttpGet("DeleteFamilyNationality")]
        public HandleErrorResponseModel DeleteFamilyNationality(int NationalityId)
        {
            var results = _familyNationalityService.DeleteFamilyNationality(NationalityId);
            return results;
        }
    }
}
