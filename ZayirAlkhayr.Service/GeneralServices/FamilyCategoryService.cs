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

namespace ZayirAlkhayr.Service.GeneralServices
{
    public class FamilyCategoryService: IFamilyCategoryService
    {
        private readonly ZADbContext _Context;
        private readonly ISQLHelper _sQLHelper;
        public FamilyCategoryService(ZADbContext context, ISQLHelper sQLHelper)
        {
            _Context = context;
            _sQLHelper = sQLHelper;
        }

        public DataTable GetAllFamilyCategoryData(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@IsFilter", false);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllFamilyCategoryDataWithFilters", Params);
            return dt;
        }

        public List<FilterModel> GetAllFamilyCategoryFilter(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@IsFilter", true);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllFamilyCategoryDataWithFilters", Params);
            var Filters = _sQLHelper.GroupingFilters(dt);
            return Filters;
        }

        public HandleErrorResponseModel AddNewFamilyCategory(FamilyCategories Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var CategoryObj = new FamilyCategories();
                CategoryObj.Name = Model.Name;
                CategoryObj.InsertUser = Model.InsertUser;
                CategoryObj.InsertDate = DateTime.Now.AddHours(1);

                _Context.FamilyCategories.Add(CategoryObj);
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

        public HandleErrorResponseModel UpdateFamilyCategory(FamilyCategories Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var CategoryObj = _Context.FamilyCategories.FirstOrDefault(x => x.Id == Model.Id);
                CategoryObj.Name = Model.Name;
                CategoryObj.UpdateUser = Model.InsertUser;
                CategoryObj.UpdateDate = DateTime.Now.AddHours(1);

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

        public HandleErrorResponseModel DeleteFamilyCategory(int CategoryId)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Category = _Context.FamilyCategories.FirstOrDefault(i => i.Id == CategoryId);

                _Context.FamilyCategories.Remove(Category);
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
