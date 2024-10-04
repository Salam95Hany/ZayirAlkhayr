using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface.GeneralServices
{
    public interface IFamilyPatientService
    {
        DataTable GetAllFamilyPatientData(PagingFilterModel PagingFilter);
        List<FilterModel> GetAllFamilyPatientFilter(PagingFilterModel PagingFilter);
        HandleErrorResponseModel AddNewFamilyPatient(FamilyPatientTypes Model);
        HandleErrorResponseModel UpdateFamilyPatient(FamilyPatientTypes Model);
        HandleErrorResponseModel DeleteFamilyPatient(int PatientId);
    }
}
