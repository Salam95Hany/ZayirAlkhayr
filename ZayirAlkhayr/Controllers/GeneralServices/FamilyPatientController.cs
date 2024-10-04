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
    public class FamilyPatientController : ControllerBase
    {
        private readonly IFamilyPatientService _familyPatientService;
        public FamilyPatientController(IFamilyPatientService familyPatientService)
        {
            _familyPatientService = familyPatientService;
        }

        [HttpPost("GetAllFamilyPatientData")]
        public DataTable GetAllFamilyPatientData(PagingFilterModel PagingFilter)
        {
            var results = _familyPatientService.GetAllFamilyPatientData(PagingFilter);
            return results;
        }

        [HttpPost("GetAllFamilyPatientFilter")]
        public List<FilterModel> GetAllFamilyPatientFilter(PagingFilterModel PagingFilter)
        {
            var results = _familyPatientService.GetAllFamilyPatientFilter(PagingFilter);
            return results;
        }

        [HttpPost("AddNewFamilyPatient")]
        public HandleErrorResponseModel AddNewFamilyPatient(FamilyPatientTypes Model)
        {
            var results = _familyPatientService.AddNewFamilyPatient(Model);
            return results;
        }

        [HttpPost("UpdateFamilyPatient")]
        public HandleErrorResponseModel UpdateFamilyPatient(FamilyPatientTypes Model)
        {
            var results = _familyPatientService.UpdateFamilyPatient(Model);
            return results;
        }

        [HttpGet("DeleteFamilyPatient")]
        public HandleErrorResponseModel DeleteFamilyPatient(int PatientId)
        {
            var results = _familyPatientService.DeleteFamilyPatient(PatientId);
            return results;
        }
    }
}
