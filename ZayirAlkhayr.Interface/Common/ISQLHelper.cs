using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;

namespace ZayirAlkhayr.Interface.Common
{
    public interface ISQLHelper
    {
        DataTable ExecuteDataTable(string commandText, params SqlParameter[] Parameters);
        DataSet ExecuteDataset(string commandText, SqlParameter[] commandParameters);
        List<TElement> SQLQuery<TElement>(string commandText, params SqlParameter[] parameters);
        List<FilterModel> GroupingFilters(DataTable dt);
        DataTable ConvertFilterModelToDataTable(List<FilterModel> FilterList);
        int GenerateCode();
    }
}
