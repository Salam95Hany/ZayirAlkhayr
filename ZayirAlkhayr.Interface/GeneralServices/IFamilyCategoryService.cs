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
    public interface IFamilyCategoryService
    {
        DataTable GetAllFamilyCategoryData(PagingFilterModel PagingFilter);
        List<FilterModel> GetAllFamilyCategoryFilter(PagingFilterModel PagingFilter);
        HandleErrorResponseModel AddNewFamilyCategory(FamilyCategories Model);
        HandleErrorResponseModel UpdateFamilyCategory(FamilyCategories Model);
        HandleErrorResponseModel DeleteFamilyCategory(int CategoryId);
    }
}
