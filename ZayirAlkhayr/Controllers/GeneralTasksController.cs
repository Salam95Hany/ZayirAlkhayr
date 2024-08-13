using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface;
using ZayirAlkhayr.Service;

namespace ZayirAlkhayr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralTasksController : ControllerBase
    {
        private readonly IGeneralTasksService _generalTasksService;
        public GeneralTasksController(IGeneralTasksService generalTasksService)
        {
            _generalTasksService = generalTasksService;
        }

        [HttpPost("GetAllGeneralTasksData")]
        public DataTable GetAllGeneralTasksData(PagingFilterModel PagingFilter)
        {
            var results = _generalTasksService.GetAllGeneralTasksData(PagingFilter);
            return results;
        }

        [HttpPost("GetAllGeneralTasksFilter")]
        public List<FilterModel> GetAllGeneralTasksFilter(PagingFilterModel PagingFilter)
        {
            var results = _generalTasksService.GetAllGeneralTasksFilter(PagingFilter);
            return results;
        }

        [HttpGet("GetAllUserTasks")]
        public DataTable GetAllUserTasks(string UserId)
        {
            var results = _generalTasksService.GetAllUserTasks(UserId);
            return results;
        }

        [HttpPost("AddNewGeneralTask")]
        public HandleErrorResponseModel AddNewGeneralTask(GeneralTasks Model)
        {
            var results = _generalTasksService.AddNewGeneralTask(Model);
            return results;
        }

        [HttpPost("UpdateGeneralTask")]
        public HandleErrorResponseModel UpdateGeneralTask(GeneralTasks Model)
        {
            var results = _generalTasksService.UpdateGeneralTask(Model);
            return results;
        }

        [HttpGet("DeleteGeneralTask")]
        public HandleErrorResponseModel DeleteGeneralTask(int TaskId)
        {
            var results = _generalTasksService.DeleteGeneralTask(TaskId);
            return results;
        }

        [HttpGet("ConvertTaskStatus")]
        public HandleErrorResponseModel ConvertTaskStatus(int TaskId, int StatusId)
        {
            var results = _generalTasksService.ConvertTaskStatus(TaskId, StatusId);
            return results;
        }
    }
}
