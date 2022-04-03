using FastMember;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace EAudit.DAO
{
   
    public class TSqlHelper
    {
        /// <summary>
        ///
        /// </summary>
        private const string SP_COMMAND_PREFIX = "EXEC dbo.";

        /// <summary>
        ///
        /// </summary>
        /// <param name="_spName"></param>
        /// <returns></returns>
        public static string spName(string _spName)
        {
            return String.Concat(SP_COMMAND_PREFIX, _spName);
        }
    }

    public class CredentialInfo
    {
        /// <summary>
        ///
        /// </summary>
        public string DbHost { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string DbName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string UserId { get; set; } = "";
        /// <summary>
        ///
        /// </summary>
        public string UserPass { get; set; } = "";
        /// <summary>
        ///
        /// </summary>
        public bool IntegratedSecurity { get; set; } = false;
    }
    public class SqlParam
    {
        /// <summary>
        ///
        /// </summary>
        public string ParamType { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ParamValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ParamDirection { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ParamName { get; set; }
    }

    public class MSSSQLParameter
    {
        public string paramName { get; set; }
        public object paramValue { get; set; }
        public SqlDbType paramType { get; set; }
        public int? paramSize { get; set; }
    }

    public class MSSQLDataAccess
    {
        public IConfiguration Configuration { get; }
        public IOptions<AppSettings> _options;


        #region Variables Initialize
        /// <summary>
        ///
        /// </summary>
        public SqlConnectionStringBuilder _SqlConnectionStringBuilder { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string SQLConnectionString { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string DbName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string UserId { get; set; } = "";
        /// <summary>
        ///
        /// </summary>
        public string UserPass { get; set; } = "";

        internal string BuildQueryString(object tsqlHelper)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        public bool IntegratedSecurity { get; set; } = false;
        /// <summary>
        ///
        /// </summary>
        public List<SqlParam> SQLParams { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string SQLQuery { get; set; }
        private string spName;
        private SqlConnection _connection;

        #endregion

        #region Constructor
        /// <summary>
        ///
        /// </summary>
        public MSSQLDataAccess(IOptions<AppSettings> options)
        {
            _options = options;
            SQLConnectionString = _options.Value.DefaultConnection;
        }

        

        public MSSQLDataAccess setSpName(string _name)
        {
            this.spName = String.Concat("EXEC dbo.", _name);
            return this;
        }

        #endregion

        /// <summary>
        ///
        /// </summary>
        /// <param name="credential"></param>
        public void _SetConnectionString(CredentialInfo credential)
        {
            SqlConnectionStringBuilder ConnectionString = new SqlConnectionStringBuilder
            {
                DataSource = $"{credential.DbHost}",
                InitialCatalog = credential.DbName,
                UserID = credential.UserId,
                Password = credential.UserPass,
                IntegratedSecurity = credential.IntegratedSecurity
            };
            _SqlConnectionStringBuilder = ConnectionString;
            // _SqlConnectionStringBuilder = ConfigurationManager.ConnectionStrings["HOMConnectionString"].ConnectionString;
        }

        #region Get schema (columns) from query
        /// <summary>
        ///
        /// </summary>
        /// <param name="Query"></param>
        /// <returns></returns>
        private DataTable GetSchemaOfQuery(string Query)
        {
            DataTable schema = null;

            // using (SqlConnection connection = new SqlConnection(_SqlConnectionStringBuilder.ConnectionString))
            using (SqlConnection connection = new SqlConnection(SQLConnectionString))
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                using (var schemaCommand = new SqlCommand(Query, connection))
                {
                    connection.Open();
                    using (var reader = schemaCommand.ExecuteReader(CommandBehavior.SchemaOnly))
                    {
                        schema = reader.GetSchemaTable();
                    }
                }
            }
            return schema;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private DataTable GetSchemaOfQuery()
        {
            DataTable schema = null;

            using (SqlConnection connection = new SqlConnection(SQLConnectionString))
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                using (var schemaCommand = new SqlCommand(SQLQuery, connection))
                {
                    connection.Open();
                    using (var reader = schemaCommand.ExecuteReader(CommandBehavior.SchemaOnly))
                    {
                        schema = reader.GetSchemaTable();
                    }
                }
            }
            return schema;
        }
        #endregion

        /// <summary>
        ///
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        #region Get columns (map object to List<string>) information from schema method

        public List<string> GetColumnsFromQuery(string query)
        {
            DataTable schema = GetSchemaOfQuery(query);
            List<string> columns = new List<string>();
            if (schema != null)
            {
                foreach (DataRow col in schema.Rows)
                {
                    columns.Add(col.Field<String>("ColumnName"));
                }
            }

            return columns;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        //public List<string> GetColumnsFromQuery()
        //{
        //    DataTable schema = null;
        //    schema = this.GetSchemaOfQuery(SQLQuery);
        //    List<string> columns = new List<string>();
        //    foreach (DataRow col in schema.Rows)
        //    {
        //        columns.Add(col.Field<String>("ColumnName"));
        //    }
        //    return columns;
        //}
        #endregion

        #region CRUD SERVICES

        #region Create / Update / Delete ===> Execute Non Query
        public void Execute(string sqlQuery)
        {
            using (SqlConnection connection = new SqlConnection(SQLConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlQuery, connection);

                try
                {
                    command.ExecuteNonQuery();
                }
                finally
                {
                    // Always call Close when done reading.
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Exec query
        /// </summary>
        /// <param name="spName">Nama stored Procedure</param>
        /// <param name="parameterList">Parameter list: List of MSSSQLParameter </param>
        public void ExecuteNonQuery(string spName, List<MSSSQLParameter> parameterList)
        {
            SqlConnection connection = new SqlConnection(SQLConnectionString);
            this.spName = spName;
            this._connection = connection;
            try
            {
                connection.Open();
                SqlCommand sqlCommand = this.BuildCommandParameter(parameterList);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }

        public IQueryable<object> ExecAsIQueryable(string strSQl)
        {
            List<object> dictionary = new List<object>();
            var Columns = new List<string>();
            string strSQL = SQLQuery;
            Columns = this.GetColumnsFromQuery(SQLQuery);
            using (SqlConnection connection = new SqlConnection(SQLConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(SQLQuery, connection);
                SqlDataReader rowsReader = command.ExecuteReader();
                try
                {
                    while (rowsReader.Read())
                    {
                        Dictionary<string, dynamic> d = new Dictionary<string, dynamic>();
                        for (var index = 0; index < Columns.Count(); index++)
                        {
                            if (Columns[index].ToString().ToLower().Contains("date"))
                            {
                                DateTime tmp = DateTime.Parse(rowsReader[index].ToString());
                                d.Add(Columns[index], tmp.ToString("dd/MM/yyyy"));
                            }
                            else
                            {
                                d.Add(Columns[index], rowsReader[index].ToString());
                            }

                        }
                        dictionary.Add(d);
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    rowsReader.Close();
                }
            }
            return dictionary.AsQueryable();
        }
        #endregion

        #region Reader

        /// <summary>
        /// Get record count
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public int GetCount(string sqlQuery)
        {
            int rowsCount = 0;
            using (SqlConnection connection = new SqlConnection(SQLConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                SqlDataReader rowsReader = command.ExecuteReader();
                try
                {
                    if (rowsReader.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(rowsReader);
                        rowsCount = dt.Rows.Count;
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    rowsReader.Close();
                }
            }
            return rowsCount;
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <returns>Array of</returns>
        public IQueryable<object> Read()
        {
            List<object> dictionary = new List<object>();
            var Columns = new List<string>();
            string strSQL = SQLQuery;
            Columns = this.GetColumnsFromQuery(strSQL);
            using (SqlConnection connection = new SqlConnection(SQLConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(strSQL, connection);
                SqlDataReader rowsReader = command.ExecuteReader();
                try
                {

                    while (rowsReader.Read())
                    {
                        Dictionary<string, dynamic> d = new Dictionary<string, dynamic>();
                        for (var index = 0; index < Columns.Count(); index++)
                        {
                            d.Add(Columns[index], rowsReader[index].ToString());
                        }
                        dictionary.Add(d);
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    rowsReader.Close();
                }
            }
            return dictionary.AsQueryable();
        }

        public IQueryable<object> Read(IOptions<AppSettings> option, string spName, List<SqlParam> parameterList)
        {
            SetOptions(option);
            string strSQL = this.BuildQueryString(TSqlHelper.spName(spName), parameterList);
            IQueryable<object> result = this.Read(strSQL);
            return result;
        }
        public IQueryable<object> Read(string spName, List<SqlParam> parameterList)
        {
            string strSQL = this.BuildQueryString(TSqlHelper.spName(spName), parameterList);
            IQueryable<object> result = this.Read(strSQL);
            return result;
        }

        private void SetOptions(IOptions<AppSettings> options)
        {
            _options = options;
            SQLConnectionString = _options.Value.DefaultConnection;
        }

        /// <summary>
        /// Mendapatkan data record berdasarkan query
        /// </summary>
        /// <param name="sqlQuery">DML</param>
        /// <returns></returns>
        public IQueryable<object> Read(string sqlQuery)
        {
            List<object> dictionary = new List<object>();
            var Columns = GetColumnsFromQuery(sqlQuery);
            using (SqlConnection connection = new SqlConnection(SQLConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                SqlDataReader rowsReader = command.ExecuteReader();
                try
                {

                    while (rowsReader.Read())
                    {
                        Dictionary<string, dynamic> d = new Dictionary<string, dynamic>();
                        for (var index = 0; index < Columns.Count(); index++)
                        {
                            d.Add(Columns[index], rowsReader[index].ToString());
                        }
                        dictionary.Add(d);
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    rowsReader.Close();
                }
            }
            return dictionary.AsQueryable();
        }

        /// <summary>
        /// [Masih dalam proses pengembangan] Mendapatkan data dan output total records
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="TotalRecords"></param>
        /// <returns></returns>
        public IQueryable<object> Read(string sqlQuery, out Int32? TotalRecords)
        {
            List<object> dictionary = new List<object>();
            var Columns = new List<string>();
            string strSQL = sqlQuery;
            Columns = this.GetColumnsFromQuery(sqlQuery);
            using (SqlConnection connection = new SqlConnection(SQLConnectionString))
            {


                connection.Open();
                SqlCommand command = new SqlCommand("dbo.TransactionType_Get", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@Name";
                paramName.Value = DBNull.Value;
                paramName.SqlDbType = SqlDbType.VarChar;
                paramName.Size = 255;
                command.Parameters.Add(paramName);

                SqlParameter paramPageNumber = new SqlParameter();
                paramPageNumber.ParameterName = "@PageNumber";
                paramPageNumber.Value = 1;
                paramPageNumber.SqlDbType = SqlDbType.Int;
                command.Parameters.Add(paramPageNumber);

                SqlParameter paramPageSize = new SqlParameter();
                paramPageSize.ParameterName = "@PageSize";
                paramPageSize.Value = 5;
                paramPageSize.SqlDbType = SqlDbType.Int;
                command.Parameters.Add(paramPageSize);

                SqlParameter paramTotalRecords = new SqlParameter();
                paramTotalRecords.ParameterName = "@TotalRecords";
                paramTotalRecords.Direction = ParameterDirection.Output;
                paramTotalRecords.SqlDbType = SqlDbType.Int;
                paramTotalRecords.Value = DBNull.Value;
                command.Parameters.Add(paramTotalRecords);

                command.ExecuteNonQuery();



                //SqlDataReader rowsReader = new SqlDataReader(); //command.ExecuteReader();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = command;

                DataTable dtTable = new DataTable();
                sqlDataAdapter.Fill(dtTable);
                if (dtTable != null)
                {
                    foreach (DataRow dr in dtTable.Rows)
                    {
                        Dictionary<string, dynamic> d = new Dictionary<string, dynamic>();
                        for (var index = 0; index < Columns.Count(); index++)
                        {
                            d.Add(Columns[index], dr[index].ToString());
                        }
                        dictionary.Add(d);
                    }
                }


                TotalRecords = command.Parameters["@TotalRecords"].Value as Int32?;

                //try
                //{

                //    while (rowsReader.Read())
                //    {
                //        Dictionary<string, dynamic> d = new Dictionary<string, dynamic>();
                //        for (var index = 0; index < Columns.Count(); index++)
                //        {
                //            d.Add(Columns[index], rowsReader[index].ToString());
                //        }
                //        dictionary.Add(d);
                //    }
                //}
                //finally
                //{
                //    // Always call Close when done reading.
                //    rowsReader.Close();
                //}
            }
            return dictionary.AsQueryable();
        }

        public List<Dictionary<string, dynamic>> ReadDictAsList(string sqlQuery)
        {
            List<Dictionary<string, dynamic>> dictionary = new List<Dictionary<string, dynamic>>();
            var Columns = new List<string>();
            string strSQL = sqlQuery;
            Columns = this.GetColumnsFromQuery(sqlQuery);
            if (Columns != null && Columns.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(SQLConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    SqlDataReader rowsReader = command.ExecuteReader();
                    try
                    {

                        while (rowsReader.Read())
                        {
                            Dictionary<string, dynamic> d = new Dictionary<string, dynamic>();
                            for (var index = 0; index < Columns.Count(); index++)
                            {
                                d.Add(Columns[index], rowsReader[index].ToString());
                            }
                            dictionary.Add(d);
                        }
                    }
                    finally
                    {
                        // Always call Close when done reading.
                        rowsReader.Close();
                    }
                }
            }

            return dictionary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public Dictionary<string, dynamic> ReadDict(string sqlQuery)
        {
            Dictionary<string, dynamic> dictionary = new Dictionary<string, dynamic>();
            var Columns = new List<string>();
            string strSQL = sqlQuery;
            Columns = this.GetColumnsFromQuery(sqlQuery);
            if (Columns != null && Columns.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(SQLConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    SqlDataReader rowsReader = command.ExecuteReader();
                    try
                    {

                        while (rowsReader.Read())
                        {
                            Dictionary<string, dynamic> d = new Dictionary<string, dynamic>();
                            for (var index = 0; index < Columns.Count(); index++)
                            {
                                d.Add(Columns[index], rowsReader[index].ToString());
                            }
                            dictionary = d;
                        }
                    }
                    finally
                    {
                        // Always call Close when done reading.
                        rowsReader.Close();
                    }
                }
            }

            return dictionary;
        }

        public Dictionary<string, dynamic> ReadAsyncDict(string sqlQuery)
        {
            Dictionary<string, dynamic> dictionary = new Dictionary<string, dynamic>();
            var Columns = new List<string>();
            string strSQL = sqlQuery;
            Columns = this.GetColumnsFromQuery(sqlQuery);
            if (Columns != null && Columns.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(SQLConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    SqlDataReader rowsReader = command.ExecuteReader();
                    try
                    {

                        while (rowsReader.Read())
                        {
                            Dictionary<string, dynamic> d = new Dictionary<string, dynamic>();
                            for (var index = 0; index < Columns.Count(); index++)
                            {
                                d.Add(Columns[index], rowsReader[index].ToString());
                            }
                            dictionary = d;
                        }
                    }
                    finally
                    {
                        // Always call Close when done reading.
                        rowsReader.Close();
                    }
                }
            }

            return dictionary;
        }

        #endregion

        #endregion

        #region Build complete SQL query if it has parameters

        //public IQueryable<T> ShowData<T>(this T instance)
        //{
        //    List<T> dictionary = new List<T>();
        //    var Columns = new List<string>();
        //    string strSQL = SQLQuery;
        //    Columns = this.GetColumnsFromQuery(strSQL);
        //    using (SqlConnection connection = new SqlConnection(SQLConnectionString))
        //    {
        //        connection.Open();
        //        SqlCommand command = new SqlCommand(strSQL, connection);
        //        SqlDataReader rowsReader = command.ExecuteReader();
        //        try
        //        {

        //            while (rowsReader.Read())
        //            {
        //                Dictionary<string, dynamic> source = new Dictionary<string, dynamic>();
        //                for (var index = 0; index < Columns.Count(); index++)
        //                {
        //                    source.Add(Columns[index], rowsReader[index].ToString());
        //                }
        //                T xx = DictionaryToObject<T>(source);
        //                //dictionary.Add(source);
        //            }
        //        }
        //        finally
        //        {
        //            // Always call Close when done reading.
        //            rowsReader.Close();
        //        }
        //    }
        //    return dictionary.AsQueryable();
        //}

        /// <summary>
        ///
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public string BuildQueryString(string sqlQuery, List<SqlParam> sqlParams)
        {
            string sql = sqlQuery + " "; //EXEC dbo.SP
            string paramCol = "";

            if (sqlParams.Count > 0)
            {
                foreach (var p in sqlParams)
                {
                    switch (p.ParamType)
                    {
                        case "INT":
                            if (string.IsNullOrEmpty(p.ParamValue))
                                paramCol += "NULL,";
                            else
                                paramCol += p.ParamValue + ",";
                            break;
                        case "BIT":
                            if (string.IsNullOrEmpty(p.ParamValue))
                                paramCol += "0,";
                            else
                                paramCol += p.ParamValue + ",";
                            break;
                        case "STRING":
                        case "DT":
                            if (string.IsNullOrEmpty(p.ParamValue))
                                paramCol += "NULL,";
                            else
                                paramCol += "'" + p.ParamValue + "',";
                            break;
                    }
                }
                sql += paramCol.Remove(paramCol.Length - 1);
            }
            return sql;
        }

        private SqlCommand BuildCommandParameter(List<MSSSQLParameter> sqlParams)
        {
            SqlCommand command = new SqlCommand(this.spName, this._connection);
            for (int idx = 0; idx < sqlParams.Count; idx++)
            {
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@" + sqlParams[idx].paramName;
                parameter.Value = sqlParams[idx].paramValue;
                parameter.SqlDbType = sqlParams[idx].paramType;// DBNull.Value;

                if (sqlParams[idx].paramSize.HasValue)
                {
                    parameter.Size = sqlParams[idx].paramSize.Value;// DBNull.Value;
                }
                else
                {
                    if (sqlParams[idx].paramType == SqlDbType.VarChar)
                    {
                        parameter.Size = 255;
                    }
                }

                command.Parameters.Add(parameter);
            }
            return command;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public string BuildQueryString(List<SqlParam> sqlParams)
        {
            string sql = SQLQuery + " ";
            string paramCol = "";

            if (sqlParams.Count > 0)
            {
                foreach (var p in sqlParams)
                {
                    switch (p.ParamType)
                    {
                        case "INT":
                            paramCol += p.ParamValue + ",";
                            break;
                        case "BIT":
                            paramCol += p.ParamValue + ",";
                            break;
                        case "STRING":
                        case "DT":
                            paramCol += "'" + p.ParamValue + "',";
                            break;
                    }
                }
                sql += paramCol.Remove(paramCol.Length - 1);
            }
            return sql;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public string BuildQueryString(string sqlQuery)
        {
            string sql = sqlQuery + " ";
            string paramCol = "";

            if (SQLParams != null && SQLParams.Count > 0)
            {
                foreach (var p in SQLParams)
                {
                    switch (p.ParamType)
                    {
                        case "INT":
                            paramCol += p.ParamValue + ",";
                            break;
                        case "BIT":
                            paramCol += p.ParamValue + ",";
                            break;
                        case "STRING":
                        case "DT":
                            paramCol += "'" + p.ParamValue + "',";
                            break;
                    }
                }
                sql += paramCol.Remove(paramCol.Length - 1);
            }
            return sql;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string BuildQueryString()
        {
            string sql = SQLQuery + " ";
            string paramCol = "";

            if (SQLParams != null && SQLParams.Count > 0)
            {
                foreach (var p in SQLParams)
                {
                    switch (p.ParamType)
                    {
                        case "INT":
                            paramCol += p.ParamValue + ",";
                            break;
                        case "BIT":
                            paramCol += p.ParamValue + ",";
                            break;
                        case "STRING":
                        case "DT":
                            paramCol += "'" + p.ParamValue + "',";
                            break;
                    }
                }
                sql += paramCol.Remove(paramCol.Length - 1);
            }
            return sql;
        }
        #endregion

    }
    public class DataTypeConstant
    {
        public const string STR = "STRING";
        public const string DTTIME = "DT";
        public const string BOOLEAN = "BL";
        public const string NUMERIC = "INT";
    }
}
