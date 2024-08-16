using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface.Tasks
{
    public interface IGeneralTasksService
    {
        DataTable GetAllGeneralTasksData(PagingFilterModel PagingFilter);
        List<FilterModel> GetAllGeneralTasksFilter(PagingFilterModel PagingFilter);
        DataTable GetAllUserTasks(string UserId);
        HandleErrorResponseModel AddNewGeneralTask(GeneralTasks Model);
        HandleErrorResponseModel UpdateGeneralTask(GeneralTasks Model);
        HandleErrorResponseModel DeleteGeneralTask(int TaskId);
        HandleErrorResponseModel ConvertTaskStatus(int TaskId, int StatusId);
    }
}
