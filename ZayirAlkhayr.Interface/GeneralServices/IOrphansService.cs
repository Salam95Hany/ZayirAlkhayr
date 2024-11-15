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
    public interface IOrphansService
    {
        List<OrphansDetailsModel> GetAllFamilyStatusOrphansType(string SearchText);
        DataTable GetAllOrphansData(PagingFilterModel PagingFilter);
        List<FilterModel> GetAllOrphansFilters(PagingFilterModel PagingFilter);
        HandleErrorResponseModel AddNewOrphans(Orphans Model);
        HandleErrorResponseModel UpdateOrphans(Orphans Model);
        HandleErrorResponseModel DeleteOrphans(int OrphansId);
        HandleErrorResponseModel AddUpdateBenefactorOrphans(BeneFactorOrphansModel Model);
        HandleErrorResponseModel DeleteBenefactorOrphans(int OrphansId);
        List<OrphansDetails> GetOrphanDetailsByFamilyId(int FamilyStatusId);
    }
}
