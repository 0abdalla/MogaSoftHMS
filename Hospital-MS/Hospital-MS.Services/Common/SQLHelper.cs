using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Data;
using System.Reflection;
using Hospital_MS.Interfaces.Common;

namespace Hospital_MS.Services.Common
{
    public class SQLHelper : ISQLHelper
    {
        private readonly IConfiguration _configuration;
        int Timeout = 9999;
        private string ConnectionString;
        public SQLHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
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

        public async Task<DataTable> ExecuteDataTableAsync(string commandText, params SqlParameter[] Parameters)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 1200;

                if (Parameters != null && Parameters.Length > 0)
                    command.Parameters.AddRange(Parameters);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    return dt;
                }
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
    }
}

