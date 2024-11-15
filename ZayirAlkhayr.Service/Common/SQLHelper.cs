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
using System.Reflection;

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

        public List<TElement> SQLQuery<TElement>(string commandText, params SqlParameter[] parameters)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, sqlConn))
                {
                    cmd.CommandText = commandText;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = (int)TimeSpan.FromMinutes(5).TotalSeconds;
                    foreach (var parameter in parameters)
                    {
                        var paramter = cmd.CreateParameter();
                        paramter.ParameterName = parameter.ParameterName;
                        paramter.Value = parameter.Value;
                        if (!string.IsNullOrEmpty(parameter.TypeName))
                        {
                            paramter.SqlDbType = SqlDbType.Structured;
                            paramter.TypeName = parameter.TypeName;
                        }
                        cmd.Parameters.Add(paramter);
                    }

                    sqlConn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        var result = MapToList<TElement>(reader);
                        sqlConn.Close();
                        return result;
                    }
                }
            }
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

        public int ExecuteScalar(string procName, params SqlParameter[] sqlParameters)
        {
            int value = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    if (sqlParameters != null)
                        sqlCommand.Parameters.AddRange(sqlParameters);
                    sqlCommand.Connection = con;
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = procName;
                    con.Open();
                    value = int.Parse(sqlCommand.ExecuteScalar().ToString());
                }
            }
            return value;
        }

        private List<T> MapToList<T>(DbDataReader dr)
        {
            var objList = new List<T>();
            var props = typeof(T).GetRuntimeProperties();

            List<string> drColumnsName = new List<string>();
            for (int i = 0; i < dr.FieldCount; i++)
            {
                drColumnsName.Add(dr.GetName(i));
            }


            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    T obj = Activator.CreateInstance<T>();
                    foreach (var prop in props)
                    {
                        if (drColumnsName.Contains(prop.Name))
                        {
                            var ordinal = dr.GetOrdinal(prop.Name);
                            var val = dr.GetValue(ordinal);
                            prop.SetValue(obj, val == DBNull.Value ? null : val);
                        }
                    }
                    objList.Add(obj);
                }
            }
            return objList;
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

        public DataTable ConvertFilterModelToDataTable(List<FilterModel> FilterList)
        {
            var dt = FilterList.Select(i => new { CategoryName = i.CategoryName, ItemId = i.ItemId }).ToList().ToDataTable();
            return dt;
        }

        public int GenerateCode()
        {
            var Params = new SqlParameter[0];
            return ExecuteScalar("web.SP_GetBeneFactorCodeSequences", Params);
        }
    }
}
