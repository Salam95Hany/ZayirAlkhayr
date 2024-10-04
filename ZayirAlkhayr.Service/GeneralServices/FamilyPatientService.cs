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
    public class FamilyPatientService: IFamilyPatientService
    {
        private readonly ZADbContext _Context;
        private readonly ISQLHelper _sQLHelper;
        public FamilyPatientService(ZADbContext context, ISQLHelper sQLHelper)
        {
            _Context = context;
            _sQLHelper = sQLHelper;
        }

        public DataTable GetAllFamilyPatientData(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@IsFilter", false);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllFamilyPatientDataWithFilters", Params);
            return dt;
        }

        public List<FilterModel> GetAllFamilyPatientFilter(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@IsFilter", true);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllFamilyPatientDataWithFilters", Params);
            var Filters = _sQLHelper.GroupingFilters(dt);
            return Filters;
        }

        public HandleErrorResponseModel AddNewFamilyPatient(FamilyPatientTypes Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var PatientObj = new FamilyPatientTypes();
                PatientObj.Name = Model.Name;
                PatientObj.InsertUser = Model.InsertUser;
                PatientObj.InsertDate = DateTime.Now.AddHours(1);

                _Context.FamilyPatientTypes.Add(PatientObj);
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

        public HandleErrorResponseModel UpdateFamilyPatient(FamilyPatientTypes Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var PatientObj = _Context.FamilyPatientTypes.FirstOrDefault(x => x.Id == Model.Id);
                PatientObj.Name = Model.Name;
                PatientObj.UpdateUser = Model.InsertUser;
                PatientObj.UpdateDate = DateTime.Now.AddHours(1);

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

        public HandleErrorResponseModel DeleteFamilyPatient(int PatientId)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Patient = _Context.FamilyPatientTypes.FirstOrDefault(i => i.Id == PatientId);

                _Context.FamilyPatientTypes.Remove(Patient);
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
