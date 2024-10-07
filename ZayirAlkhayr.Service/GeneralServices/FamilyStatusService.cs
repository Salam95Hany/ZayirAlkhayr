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

        public UpdateFamilyStatusLookups GetUpdateFamilyStatusLookups(int FamilyStatusId)
        {
            var Lookups = GetFamilyStatusLookups();
            var FamilyStatus = GetFamilyStatus(FamilyStatusId);
            var FamilyIncome = GetFamilyIncome(FamilyStatusId);
            var FamilyExpenses = GetFamilyExpenses(FamilyStatusId);
            var FamilyExtraDetails = GetFamilyExtraDetails(FamilyStatusId);
            var FamilyDetails = GetFamilyDetails(FamilyStatusId);
            var FamilyPatient = GetFamilyPatient(FamilyStatusId);
            var FamilyNeeds = GetFamilyNeeds(FamilyStatusId);

            var Model = new UpdateFamilyStatusLookups
            {
                Lookups = Lookups,
                FamilyStatus = FamilyStatus,
                FamilyIncome = FamilyIncome,
                FamilyExpenses= FamilyExpenses,
                FamilyExtraDetails = FamilyExtraDetails,
                FamilyDetails = FamilyDetails,
                FamilyPatient = FamilyPatient,
                FamilyNeeds = FamilyNeeds

            };

            return Model;
        }

        public FamilyStatus GetFamilyStatus(int FamilyStatusId)
        {
            var results = _Context.FamilyStatus.FirstOrDefault(i => i.Id == FamilyStatusId);
            return results;
        }

        public FamilyIncome GetFamilyIncome(int FamilyStatusId)
        {
            var results = _Context.FamilyIncome.FirstOrDefault(i => i.FamilyStatusId == FamilyStatusId);
            return results;
        }

        public FamilyExpenses GetFamilyExpenses(int FamilyStatusId)
        {
            var results = _Context.FamilyExpenses.FirstOrDefault(i => i.FamilyStatusId == FamilyStatusId);
            return results;
        }

        public FamilyExtraDetails GetFamilyExtraDetails(int FamilyStatusId)
        {
            var results = _Context.FamilyExtraDetails.FirstOrDefault(i => i.FamilyStatusId == FamilyStatusId);
            return results;
        }

        public List<FamilyDetails> GetFamilyDetails(int FamilyStatusId)
        {
            var results = _Context.FamilyDetails.Where(i => i.FamilyStatusId == FamilyStatusId).ToList();
            return results;
        }

        public List<FamilyPatient> GetFamilyPatient(int FamilyStatusId)
        {
            var results = _Context.FamilyPatient.Where(i => i.FamilyStatusId == FamilyStatusId).ToList();
            return results;
        }

        public List<FamilyNeeds> GetFamilyNeeds(int FamilyStatusId)
        {
            var results = _Context.FamilyNeeds.Where(i => i.StatusId == FamilyStatusId).ToList();
            return results;
        }
    }
}
