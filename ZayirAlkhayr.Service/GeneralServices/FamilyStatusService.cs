using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.Common;
using ZayirAlkhayr.Interface.GeneralServices;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service.GeneralServices
{
    public class FamilyStatusService : IFamilyStatusService
    {
        private readonly ZADbContext _Context;
        private readonly ISQLHelper _sQLHelper;
        public FamilyStatusService(ZADbContext context, ISQLHelper sQLHelper)
        {
            _Context = context;
            _sQLHelper = sQLHelper;
        }

        public DataTable GetAllFamilyStatusData(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@IsFilter", false);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllFamilyStatusDataWithFilters", Params);
            return dt;
        }

        public List<FilterModel> GetAllFamilyStatusFilter(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@IsFilter", true);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllFamilyStatusDataWithFilters", Params);
            var Filters = _sQLHelper.GroupingFilters(dt);
            return Filters;
        }

        public FamilyStatusLookups GetFamilyStatusLookups()
        {
            var Categories = GetFamilyCategories().GetAwaiter().GetResult();
            var Nationalities = GetFamilyNationalities().GetAwaiter().GetResult();
            var Needs = GetFamilyNeedTypes().GetAwaiter().GetResult();
            var NeedCategories = GetFamilyNeedCategories().GetAwaiter().GetResult();
            var FamilyStatusTypes = GetFamilyStatusTypes().GetAwaiter().GetResult();
            var PatientTypes = GetFamilyPatientTypes().GetAwaiter().GetResult();

            var Model = new FamilyStatusLookups
            {
                Categories = Categories,
                Nationalities = Nationalities,
                FamilyNeeds = Needs,
                FamilyNeedCategories = NeedCategories,
                StatusTypes = FamilyStatusTypes,
                PatientTypes = PatientTypes
            };

            return Model;
        }

        Task<List<FamilyCategories>> GetFamilyCategories()
        {
            var results = _Context.FamilyCategories.ToListAsync();
            return results;
        }

        Task<List<FamilyPatientTypes>> GetFamilyPatientTypes()
        {
            var results = _Context.FamilyPatientTypes.ToListAsync();
            return results;
        }

        Task<List<FamilyNationalities>> GetFamilyNationalities()
        {
            var results = _Context.FamilyNationalities.ToListAsync();
            return results;
        }

        Task<List<FamilyStatusTypes>> GetFamilyStatusTypes()
        {
            var results = _Context.FamilyStatusTypes.ToListAsync();
            return results;
        }

        Task<List<FamilyNeedTypes>> GetFamilyNeedTypes()
        {
            var results = _Context.FamilyNeedTypes.ToListAsync();
            return results;
        }

        Task<List<FamilyNeedCategories>> GetFamilyNeedCategories()
        {
            var results = _Context.FamilyNeedCategories.ToListAsync();
            return results;
        }
    }
}
