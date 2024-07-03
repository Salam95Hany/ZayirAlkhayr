using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZayirAlkhayr.Interface.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using ZayirAlkhayr.Entities.Common;

namespace ZayirAlkhayr.Service.Common
{
    public class SQLHelper : ISQLHelper
    {
        private readonly IConfiguration _configuration;
        int Timeout = 9999;
        private string ConnectionString;
        public SQLHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("DBConnection");
        }

        public DataTable ExecuteDataTable(string commandText, params SqlParameter[] Parameters)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dt = new DataTable();
                connection.Open();
                SqlCommand command = new SqlCommand();

                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = commandText;
                for (int i = 0; i < Parameters.Length; i++)
                {
                    command.Parameters.Add(Parameters[i]);

                }
                SqlDataAdapter adpater = new SqlDataAdapter(command);
                adpater.SelectCommand.CommandTimeout = 1200;
                adpater.Fill(dt);
                connection.Close();
                return dt;
            }
        }

        public DataSet ExecuteDataset(string commandText, SqlParameter[] commandParameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand();

                    PrepareCommand(connection, sqlCommand, (SqlTransaction)null, CommandType.StoredProcedure, commandText, commandParameters);
                    sqlCommand.CommandTimeout = Timeout;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    DataSet dataSet = new DataSet();
                    ((DataAdapter)sqlDataAdapter).Fill(dataSet);
                    sqlCommand.Parameters.Clear();
                    return dataSet;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void PrepareCommand(SqlConnection connection, SqlCommand command, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            command.Connection = connection;
            command.CommandTimeout = Timeout;
            command.CommandText = commandText;
            if (transaction != null)
                command.Transaction = transaction;
            command.CommandType = commandType;
            if (commandParameters == null)
                return;
            SQLHelper.AttachParameters(command, commandParameters);
        }
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            foreach (SqlParameter sqlParameter in commandParameters)
            {
                if (sqlParameter.Direction == ParameterDirection.InputOutput && sqlParameter.Value == null)
                    sqlParameter.Value = (object)DBNull.Value;
                command.Parameters.Add(sqlParameter);
            }
        }

        public List<FilterModel> GroupingFilters(DataTable dt)
        {
            List<FilterModel> List = dt.AsEnumerable().GroupBy(y => new
            {
                CategoryName = y.Field<string>("CategoryName"),
            }).Select(x => new FilterModel
            {
                CategoryName = x.Key.CategoryName,
                IsVisible = x.FirstOrDefault().Field<bool>("IsVisible"),
                FilterType = x.FirstOrDefault().Field<string>("FilterType"),
                FilterItems = x.Select(s => new FilterModel
                {
                    CategoryName = s.Field<string>("CategoryName"),
                    ItemValue = s.Field<string>("ItemValue"),
                    ItemKey = s.Field<string>("ItemKey"),
                    ItemId = s.Field<string>("ItemId")
                }).ToList()
                
            }).ToList();

            return List;
        }
    }
}
