using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Interface.Common
{
    public interface ISQLHelper
    {
        DataTable ExecuteDataTable(string commandText, params SqlParameter[] Parameters);
        DataSet ExecuteDataset(string commandText, SqlParameter[] commandParameters);
    }
}
