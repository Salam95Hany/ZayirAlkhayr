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
    public interface IFamilyNationalityService
    {
        DataTable GetAllFamilyNationalitiesData(PagingFilterModel PagingFilter);
        List<FilterModel> GetAllFamilyNationalitiesFilter(PagingFilterModel PagingFilter);
        HandleErrorResponseModel AddNewFamilyNationality(FamilyNationalities Model);
        HandleErrorResponseModel UpdateFamilyNationality(FamilyNationalities Model);
        HandleErrorResponseModel DeleteFamilyNationality(int NationalityId);
    }
}
