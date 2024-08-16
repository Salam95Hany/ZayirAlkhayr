using Microsoft.Data.SqlClient;
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
    public class FamilyNeedsService : IFamilyNeedsService
    {
        private readonly ZADbContext _Context;
        private readonly ISQLHelper _sQLHelper;
        public FamilyNeedsService(ZADbContext context, ISQLHelper sQLHelper)
        {
            _Context = context;
            _sQLHelper = sQLHelper;
        }

        public DataTable GetAllFamilyNeedTypesData(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@IsFilter", false);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllFamilyNeedTypesDataWithFilter", Params);
            return dt;
        }

        public List<FilterModel> GetAllFamilyNeedTypesFilters(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@IsFilter", true);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllFamilyNeedTypesDataWithFilter", Params);
            var Filters = _sQLHelper.GroupingFilters(dt);
            return Filters;
        }

        public DataTable GetAllFamilyNeedCategoriesData(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@IsFilter", false);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllFamilyNeedCategoriesDataWithFilter", Params);
            return dt;
        }

        public List<FilterModel> GetAllFamilyNeedCategoriesFilters(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@IsFilter", true);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllFamilyNeedCategoriesDataWithFilter", Params);
            var Filters = _sQLHelper.GroupingFilters(dt);
            return Filters;
        }

        public List<FamilyNeedCategories> GetAllFamilyNeedCategories()
        {
            var results = _Context.FamilyNeedCategories.ToList();
            return results;
        }

        public HandleErrorResponseModel AddNewFamilyNeedType(FamilyNeedTypes Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var NeedObj = new FamilyNeedTypes();
                NeedObj.CategoryId = Model.CategoryId;
                NeedObj.Name = Model.Name;
                NeedObj.InsertUser = Model.InsertUser;
                NeedObj.InsertDate = DateTime.Now.AddHours(1);

                _Context.FamilyNeedTypes.Add(NeedObj);
                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم اضافة عنصر جديد بنجاح";
                return Response;
            }
            catch (Exception)
            {
                var Response = new HandleErrorResponseModel();
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public HandleErrorResponseModel AddNewFamilyNeedCategory(FamilyNeedCategories Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var CategoryObj = new FamilyNeedCategories();
                CategoryObj.Name = Model.Name;
                CategoryObj.InsertUser = Model.InsertUser;
                CategoryObj.InsertDate = DateTime.Now.AddHours(1);

                _Context.FamilyNeedCategories.Add(CategoryObj);
                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم اضافة عنصر جديد بنجاح";
                return Response;
            }
            catch (Exception)
            {
                var Response = new HandleErrorResponseModel();
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public HandleErrorResponseModel UpdateFamilyNeedType(FamilyNeedTypes Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var NeedObj = _Context.FamilyNeedTypes.FirstOrDefault(x => x.Id == Model.Id);
                NeedObj.CategoryId = Model.CategoryId;
                NeedObj.Name = Model.Name;
                NeedObj.UpdateUser = Model.InsertUser;
                NeedObj.UpdateDate = DateTime.Now.AddHours(1);

                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم تعديل العنصر بنجاح";
                return Response;
            }
            catch (Exception)
            {
                var Response = new HandleErrorResponseModel();
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public HandleErrorResponseModel UpdateFamilyNeedCategory(FamilyNeedCategories Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var NeedObj = _Context.FamilyNeedCategories.FirstOrDefault(x => x.Id == Model.Id);
                NeedObj.Name = Model.Name;
                NeedObj.UpdateUser = Model.InsertUser;
                NeedObj.UpdateDate = DateTime.Now.AddHours(1);

                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم تعديل العنصر بنجاح";
                return Response;
            }
            catch (Exception)
            {
                var Response = new HandleErrorResponseModel();
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public HandleErrorResponseModel DeleteFamilyNeedType(int NeedTypeId)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Need = _Context.FamilyNeedTypes.FirstOrDefault(i => i.Id == NeedTypeId);

                _Context.FamilyNeedTypes.Remove(Need);
                _Context.SaveChanges();
                Response.Done = true;
                Response.Message = "تم حذف العنصر بنجاح";
                return Response;
            }
            catch (Exception)
            {
                var Response = new HandleErrorResponseModel();
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public HandleErrorResponseModel DeleteFamilyNeedCategory(int CategoryId)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Category = _Context.FamilyNeedCategories.FirstOrDefault(i => i.Id == CategoryId);

                _Context.FamilyNeedCategories.Remove(Category);
                _Context.SaveChanges();
                Response.Done = true;
                Response.Message = "تم حذف العنصر بنجاح";
                return Response;
            }
            catch (Exception)
            {
                var Response = new HandleErrorResponseModel();
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }
    }
}
