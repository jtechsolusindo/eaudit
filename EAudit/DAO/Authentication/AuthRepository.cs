using EAudit.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace EAudit.DAO.Authentication
{
    public class AuthRepository : IAuthInterface
    {
        /// <summary>
        /// DB Option
        /// </summary>
        public IOptions<AppSettings> _options;

        /// <summary>
        /// Connection string
        /// </summary>
        public string SQLConnectionString;

        public AuthRepository(IOptions<AppSettings> options)
        {
            _options = options;
            SQLConnectionString = _options.Value.DefaultConnection;
        }
        public async Task<List<UserInfo>> GetAuth(string username, string password)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            SqlParameter[] parameters = {
                new SqlParameter("@username", System.Data.SqlDbType.VarChar, 50, username.ToString()),
                new SqlParameter("@password", System.Data.SqlDbType.VarChar, 50, password.ToString())
            };

            DBAccess dbAccess = new DBAccess(_options);
            List<UserInfo> userLogin = await dbAccess.ExecuteReaderAsync<UserInfo>("SP_GET_USERLOGIN", parameters);

            return userLogin;
        }

        public async Task<List<UserInfo>> GetUserList(string npp)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            SqlParameter[] parameters = {
                new SqlParameter("@npp", System.Data.SqlDbType.VarChar, 50, npp.ToString()),
            };

            DBAccess dbAccess = new DBAccess(_options);
            List<UserInfo> userLogin = await dbAccess.ExecuteReaderAsync<UserInfo>("SP_GET_USER_BY_NPP", parameters);

            return userLogin;
        }

        public async Task SaveLog(string keterangan, string npp)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@TANGGAL_WAKTU", System.Data.SqlDbType.DateTime, 50, DateTime.Now.ToString()),
                new SqlParameter("@KETERANGAN", System.Data.SqlDbType.VarChar, 50, keterangan),
                new SqlParameter("@NPP", System.Data.SqlDbType.VarChar, 50, npp)
            };

            DBAccess dbAccess = new DBAccess(_options);
            await dbAccess.ExecuteQuery("SP_GET_USERLOGIN", parameters);
        }

    }
}
