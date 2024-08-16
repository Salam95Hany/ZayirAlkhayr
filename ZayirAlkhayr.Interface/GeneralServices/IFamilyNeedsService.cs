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
    public interface IFamilyNeedsService
    {
        DataTable GetAllFamilyNeedTypesData(PagingFilterModel PagingFilter);
        List<FilterModel> GetAllFamilyNeedTypesFilters(PagingFilterModel PagingFilter);
        DataTable GetAllFamilyNeedCategoriesData(PagingFilterModel PagingFilter);
        List<FilterModel> GetAllFamilyNeedCategoriesFilters(PagingFilterModel PagingFilter);
        List<FamilyNeedCategories> GetAllFamilyNeedCategories();
        HandleErrorResponseModel AddNewFamilyNeedType(FamilyNeedTypes Model);
        HandleErrorResponseModel AddNewFamilyNeedCategory(FamilyNeedCategories Model);
        HandleErrorResponseModel UpdateFamilyNeedType(FamilyNeedTypes Model);
        HandleErrorResponseModel UpdateFamilyNeedCategory(FamilyNeedCategories Model);
        HandleErrorResponseModel DeleteFamilyNeedType(int NeedTypeId);
        HandleErrorResponseModel DeleteFamilyNeedCategory(int CategoryId);
    }
}
