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
    public class OrphansController : ControllerBase
    {
        private readonly IOrphansService _orphansService;
        public OrphansController(IOrphansService orphansService)
        {
            _orphansService = orphansService;
        }

        [HttpGet("GetAllFamilyStatusOrphansType")]
        public List<OrphansDetailsModel> GetAllFamilyStatusOrphansType(string SearchText)
        {
            var results = _orphansService.GetAllFamilyStatusOrphansType(SearchText);
            return results;
        }

        [HttpPost("GetAllOrphansData")]
        public DataTable GetAllOrphansData(PagingFilterModel PagingFilter)
        {
            var results = _orphansService.GetAllOrphansData(PagingFilter);
            return results;
        }

        [HttpPost("GetAllOrphansFilters")]
        public List<FilterModel> GetAllOrphansFilters(PagingFilterModel PagingFilter)
        {
            var results = _orphansService.GetAllOrphansFilters(PagingFilter);
            return results;
        }

        [HttpPost("AddNewOrphans")]
        public HandleErrorResponseModel AddNewOrphans(Orphans Model)
        {
            var results = _orphansService.AddNewOrphans(Model);
            return results;
        }

        [HttpPost("UpdateOrphans")]
        public HandleErrorResponseModel UpdateOrphans(Orphans Model)
        {
            var results = _orphansService.UpdateOrphans(Model);
            return results;
        }

        [HttpGet("DeleteOrphans")]
        public HandleErrorResponseModel DeleteOrphans(int OrphansId)
        {
            var results = _orphansService.DeleteOrphans(OrphansId);
            return results;
        }

        [HttpPost("AddUpdateBenefactorOrphans")]
        public HandleErrorResponseModel AddUpdateBenefactorOrphans(BeneFactorOrphansModel Model)
        {
            var results = _orphansService.AddUpdateBenefactorOrphans(Model);
            return results;
        }

        [HttpGet("DeleteBenefactorOrphans")]
        public HandleErrorResponseModel DeleteBenefactorOrphans(int OrphansId)
        {
            var results = _orphansService.DeleteBenefactorOrphans(OrphansId);
            return results;
        }

        [HttpGet("GetOrphanDetailsByFamilyId")]
        public List<OrphansDetails> GetOrphanDetailsByFamilyId(int FamilyStatusId)
        {
            var results = _orphansService.GetOrphanDetailsByFamilyId(FamilyStatusId);
            return results;
        }
    }
}
