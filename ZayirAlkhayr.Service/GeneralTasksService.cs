using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface;
using ZayirAlkhayr.Interface.Common;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service
{
    public class GeneralTasksService : IGeneralTasksService
    {
        private readonly ZADbContext _Context;
        private readonly ISQLHelper _sQLHelper;
        public GeneralTasksService(ZADbContext context, ISQLHelper sQLHelper)
        {
            _Context = context;
            _sQLHelper = sQLHelper;
        }

        public DataTable GetAllGeneralTasksData(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);

            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@IsFilter", false);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllGeneralTasksDataWithFilter", Params);
            return dt;
        }

        public List<FilterModel> GetAllGeneralTasksFilter(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@IsFilter", true);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllGeneralTasksDataWithFilter", Params);
            var Filters = _sQLHelper.GroupingFilters(dt);
            return Filters;
        }

        public DataTable GetAllUserTasks(string UserId)
        {

            var Params = new SqlParameter[2];
            Params[0] = new SqlParameter("@Today", DateTime.Now.AddHours(1));
            Params[1] = new SqlParameter("@UserId", UserId);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllUserTasks", Params);
            return dt;
        }

        public HandleErrorResponseModel AddNewGeneralTask(GeneralTasks Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var TaskObj = new GeneralTasks();
                TaskObj.StatusId = 1;
                TaskObj.Task = Model.Task;
                TaskObj.AssignTo = Model.AssignTo == null ? Model.InsertUser : Model.AssignTo;
                TaskObj.InsertUser = Model.InsertUser;
                TaskObj.InsertDate = DateTime.Now.AddHours(1);

                _Context.GeneralTasks.Add(TaskObj);
                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم اضافة مهمة جديدة بنجاح";
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

        public HandleErrorResponseModel UpdateGeneralTask(GeneralTasks Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var TaskObj = _Context.GeneralTasks.FirstOrDefault(x => x.Id == Model.Id);
                TaskObj.Task = Model.Task;
                TaskObj.AssignTo = Model.AssignTo == null ? Model.InsertUser : Model.AssignTo;
                TaskObj.UpdateUser = Model.InsertUser;
                TaskObj.UpdateDate = DateTime.Now.AddHours(1);

                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم تعديل المهمة بنجاح";
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

        public HandleErrorResponseModel DeleteGeneralTask(int TaskId)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Task = _Context.GeneralTasks.FirstOrDefault(i => i.Id == TaskId);

                _Context.GeneralTasks.Remove(Task);
                _Context.SaveChanges();
                Response.Done = true;
                Response.Message = "تم حذف المهمة بنجاح";
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

        public HandleErrorResponseModel ConvertTaskStatus(int TaskId, int StatusId)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Task = _Context.GeneralTasks.FirstOrDefault(i => i.Id == TaskId);
                Task.StatusId = StatusId;
                _Context.SaveChanges();
                Response.Done = true;
                Response.Message = "تم تغيير الحالة بنجاح";
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
