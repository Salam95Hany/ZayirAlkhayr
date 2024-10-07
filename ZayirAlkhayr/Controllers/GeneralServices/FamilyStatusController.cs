using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Interface.GeneralServices;

namespace ZayirAlkhayr.Controllers.GeneralServices
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyStatusController : ControllerBase
    {
        private readonly IAddFamilyStatusService _addFamilyStatusService;
        private readonly IUpdateFamilyStatusService _updateFamilyStatusService;
        private readonly IFamilyStatusService _familyStatusService;
        public FamilyStatusController(IAddFamilyStatusService addFamilyStatusService, IFamilyStatusService familyStatusService, IUpdateFamilyStatusService updateFamilyStatusService)
        {
            _addFamilyStatusService = addFamilyStatusService;
            _familyStatusService = familyStatusService;
            _updateFamilyStatusService = updateFamilyStatusService;
        }

        [HttpPost("GetAllFamilyStatusData")]
        public DataTable GetAllFamilyStatusData(PagingFilterModel PagingFilter)
        {
            var results = _familyStatusService.GetAllFamilyStatusData(PagingFilter);
            return results;
        }

        [HttpPost("GetAllFamilyStatusFilter")]
        public List<FilterModel> GetAllFamilyStatusFilter(PagingFilterModel PagingFilter)
        {
            var results = _familyStatusService.GetAllFamilyStatusFilter(PagingFilter);
            return results;
        }

        [HttpGet("GetFamilyStatusLookups")]
        public FamilyStatusLookups GetFamilyStatusLookups()
        {
            var results = _familyStatusService.GetFamilyStatusLookups();
            return results;
        }

        [HttpGet("GetUpdateFamilyStatusLookups")]
        public UpdateFamilyStatusLookups GetUpdateFamilyStatusLookups(int FamilyStatusId)
        {
            var results = _familyStatusService.GetUpdateFamilyStatusLookups(FamilyStatusId);
            return results;
        }

        [HttpPost("AddNewFamilyStatus")]
        public HandleErrorResponseModel AddNewFamilyStatus(AddFamilyStatusModel Model)
        {
            var results = _addFamilyStatusService.AddNewFamilyStatus(Model);
            return results;
        }

        [HttpPost("UpdateFamilyStatus")]
        public HandleErrorResponseModel UpdateFamilyStatus(AddFamilyStatusModel Model)
        {
            var results = _updateFamilyStatusService.UpdateFamilyStatus(Model);
            return results;
        }
    }
}
