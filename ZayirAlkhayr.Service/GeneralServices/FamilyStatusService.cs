using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public FamilyStatusLookups GetFamilyStatusLookups()
        {
            var Categories = GetFamilyCategories().GetAwaiter().GetResult();
            var Nationalities = GetFamilyNationalities().GetAwaiter().GetResult();
            var NeedGroups = GetFamilyNeedCategoryGroups().GetAwaiter().GetResult();

            var Model = new FamilyStatusLookups
            {
                Categories = Categories,
                Nationalities = Nationalities,
                FamilyNeeds = NeedGroups
            };

            return Model;
        }

        Task<List<FamilyCategories>> GetFamilyCategories()
        {
            var results = _Context.FamilyCategories.ToListAsync();
            return results;
        }

        Task<List<FamilyNationalities>> GetFamilyNationalities()
        {
            var results = _Context.FamilyNationalities.ToListAsync();
            return results;
        }

        async Task<List<FamilyNeedCategoryGroups>> GetFamilyNeedCategoryGroups()
        {
            var Categories = await _Context.FamilyNeedCategories.ToListAsync();
            var Needs = await  _Context.FamilyNeedTypes.ToListAsync();
            var results = Needs.GroupBy(i => i.CategoryId).Select(x => new FamilyNeedCategoryGroups
            {
                CategoryName = Categories.FirstOrDefault(i => i.Id == x.Key).Name,
                Needs = x.ToList()
            }).ToList();

            return results;
        }
    }
}
