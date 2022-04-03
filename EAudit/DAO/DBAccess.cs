using FastMember;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EAudit.DAO
{
    public class DBAccess
    {
        public IConfiguration Configuration { get; }
        public IOptions<AppSettings> _options;
        public string _ConnString { get; set; }
        public DBAccess(IOptions<AppSettings> options)
        {
            _options = options;
            _ConnString = _options.Value.DefaultConnection;
        }
        public async Task<List<T>> ExecuteReaderAsync<T>(string storedProcedureName, SqlParameter[] sqlParameters = null) where T : class, new()
        {
            var newListObject = new List<T>();

            using (var conn = new SqlConnection(_ConnString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(storedProcedureName, conn))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Clear();
                    if (sqlParameters != null)
                    {
                        for (int idx = 0; idx < sqlParameters.Length; idx++)
                        {
                            sqlCommand.Parameters.Add(sqlParameters[idx].ParameterName, sqlParameters[0].SqlDbType).Value = sqlParameters[idx].SourceColumn;
                        }
                    }

                    await conn.OpenAsync();
                    using (var dataReader = await sqlCommand.ExecuteReaderAsync(CommandBehavior.Default))
                    {
                        if (dataReader.HasRows)
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var newObject = new T();
                                dataReader.MapDataToObject(newObject);
                                newListObject.Add(newObject);
                            }
                        }
                    }
                }
            }

            return newListObject;
        }

       

        public async Task ExecuteQuery(string storedProcedureName, SqlParameter[] sqlParameters = null)
        {
            using (var conn = new SqlConnection(_ConnString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(storedProcedureName, conn))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Clear();
                    if (sqlParameters != null)
                    {
                        for (int idx = 0; idx < sqlParameters.Length; idx++)
                        {
                            sqlCommand.Parameters.Add(sqlParameters[idx].ParameterName, sqlParameters[0].SqlDbType).Value = sqlParameters[idx].SourceColumn;
                        }
                    }

                    await conn.OpenAsync();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public async Task Execute(string query, SqlParameter[] sqlParameters = null)
        {
            using (var conn = new SqlConnection(_ConnString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, conn))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.Clear();
                    if (sqlParameters != null)
                    {
                        for (int idx = 0; idx < sqlParameters.Length; idx++)
                        {
                            sqlCommand.Parameters.Add(sqlParameters[idx].ParameterName, sqlParameters[0].SqlDbType).Value = sqlParameters[idx].SourceColumn;
                        }
                    }

                    await conn.OpenAsync();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
        public async Task<List<T>> ExecuteAsync<T>(string query, SqlParameter[] sqlParameters = null) where T : class, new()
        {
            var newListObject = new List<T>();

            using (var conn = new SqlConnection(_ConnString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, conn))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.Clear();
                    if (sqlParameters != null)
                    {
                        for (int idx = 0; idx < sqlParameters.Length; idx++)
                        {
                            sqlCommand.Parameters.Add(sqlParameters[idx].ParameterName, sqlParameters[0].SqlDbType).Value = sqlParameters[idx].SourceColumn;
                        }
                    }

                    await conn.OpenAsync();
                    using (var dataReader = await sqlCommand.ExecuteReaderAsync(CommandBehavior.Default))
                    {
                        if (dataReader.HasRows)
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var newObject = new T();
                                dataReader.MapDataToObject(newObject);
                                newListObject.Add(newObject);
                            }
                        }
                    }
                }
            }

            return newListObject;
        }


    }
    public class DBOutput
    {
        public bool status { get; set; }
        public string message { get; set; }
        public dynamic data { get; set; }
        public int lastid { get; set; }
    }

    public static class MyDataReader
    {
        public static void MapDataToObject<T>(this SqlDataReader dataReader, T newObject)
        {
            if (newObject == null) throw new ArgumentNullException(nameof(newObject));

            // Fast Member Usage
            var objectMemberAccessor = TypeAccessor.Create(newObject.GetType());
            var propertiesHashSet =
                    objectMemberAccessor
                    .GetMembers()
                    .Select(mp => mp.Name)
                    .ToHashSet(StringComparer.InvariantCultureIgnoreCase);

            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                var name = propertiesHashSet.FirstOrDefault(a => a.Equals(dataReader.GetName(i), StringComparison.InvariantCultureIgnoreCase));
                if (!String.IsNullOrEmpty(name))
                {
                    //Attention! if you are getting errors here, then double check that your model and sql have matching types for the field name.
                    //Check api.log for error message!
                    objectMemberAccessor[newObject, name]
                        = dataReader.IsDBNull(i) ? null : dataReader.GetValue(i);
                }
            }
        }
    }
}
