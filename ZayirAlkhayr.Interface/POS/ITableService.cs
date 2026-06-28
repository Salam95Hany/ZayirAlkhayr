using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface.POS
{
    public interface ITableService
    {
        Task<ApiResponseModel<List<Table>>> GetAllTable();
        Task<ApiResponseModel<string>> AddNewTable(Table Model);
        Task<ApiResponseModel<string>> UpdateTable(Table Model);
        Task<ApiResponseModel<string>> DeleteTable(int TableId);
    }
}
