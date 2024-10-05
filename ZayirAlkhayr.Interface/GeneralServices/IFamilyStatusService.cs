using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;

namespace ZayirAlkhayr.Interface.GeneralServices
{
    public interface IFamilyStatusService
    {
        FamilyStatusLookups GetFamilyStatusLookups();
        DataTable GetAllFamilyStatusData(PagingFilterModel PagingFilter);
        List<FilterModel> GetAllFamilyStatusFilter(PagingFilterModel PagingFilter);
    }
}
