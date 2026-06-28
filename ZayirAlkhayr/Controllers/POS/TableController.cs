using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.POS;

namespace ZayirAlkhayr.Controllers.POS
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;
        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }

        [HttpGet("GetAllTable")]
        public async Task<ApiResponseModel<List<Table>>> GetAllTable()
        {
            var results = await _tableService.GetAllTable();
            return results;
        }

        [HttpPost("AddNewTable")]
        public async Task<ApiResponseModel<string>> AddNewTable(Table Model)
        {
            var results = await _tableService.AddNewTable(Model);
            return results;
        }

        [HttpPost("UpdateTable")]
        public async Task<ApiResponseModel<string>> UpdateTable(Table Model)
        {
            var results = await _tableService.UpdateTable(Model);
            return results;
        }

        [HttpGet("DeleteTable")]
        public async Task<ApiResponseModel<string>> DeleteTable(int TableId)
        {
            var results = await _tableService.DeleteTable(TableId);
            return results;
        }
    }
}
