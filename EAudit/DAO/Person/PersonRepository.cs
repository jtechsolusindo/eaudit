using EAudit.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EAudit.DAO.Person
{
    public class PersonRepository : IPerson
    {
        private string _roleType;

        public IOptions<AppSettings> _options;

        public PersonRepository(IOptions<AppSettings> options)
        {
            _options = options;
        }

        private string useDBObject(string role)
        {
            if (role == "Admin")
            {
                _roleType = "SP_GET_AUDITOR_LIST";
            }
            else if (role == "Auditee")
            {
                _roleType = "SP_AUDITEE_LIST";
            }
            else if (role == "Auditor")
            {
                _roleType = "SP_AUDITOR_LIST";
            }
            return _roleType;
        }
        public async Task<List<AuditorAuditee>> getPersonList(string search, string role = "Admin/Auditee/Auditor")
        {
            DBOutput output = new DBOutput();
            output.status = true;
            SqlParameter[] parameters = {
                new SqlParameter("@search", System.Data.SqlDbType.VarChar, 50, search)
            };

            DBAccess dbAccess = new DBAccess(_options);
            List<AuditorAuditee> list = await dbAccess.ExecuteReaderAsync<AuditorAuditee>(this.useDBObject(role), parameters);

            return list;
        }

        public async Task<AuditorAuditee> getAuditorRow(AuditorAuditee filter)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.Int,10, filter.ID.Value.ToString())
            };

            DBAccess dbAccess = new DBAccess(_options);
            List<AuditorAuditee> list = await dbAccess.ExecuteReaderAsync<AuditorAuditee>("SP_AUDITOR_ROW", parameters);

            return list[0];
        }

        public async Task<AuditorAuditee> getAuditeeRow(AuditorAuditee filter)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.Int,10, filter.ID.Value.ToString())
            };

            DBAccess dbAccess = new DBAccess(_options);
            List<AuditorAuditee> list = await dbAccess.ExecuteReaderAsync<AuditorAuditee>("SP_AUDITEE_ROW", parameters);

            return list[0];
        }

        /// <summary>
        /// Karyawan yang belum menjadi auditor
        /// </summary>
        /// <returns></returns>
        public async Task<List<EmployeeUnAssigned>> getEmployeeUnListed()
        {
            DBOutput output = new DBOutput();
            output.status = true;

            DBAccess dbAccess = new DBAccess(_options);
            List<EmployeeUnAssigned> list = await dbAccess.ExecuteReaderAsync<EmployeeUnAssigned>("SP_GET_KARYAWAN_NOT_ASSIGNED");

            return list;
        }
        /// <summary>
        /// Bagian yang belum menjadi Auditee
        /// </summary>
        /// <returns></returns>
        public async Task<List<EmployeeUnAssigned>> getUnlistedAuditee()
        {
            DBOutput output = new DBOutput();
            output.status = true;

            DBAccess dbAccess = new DBAccess(_options);
            List<EmployeeUnAssigned> list = await dbAccess.ExecuteReaderAsync<EmployeeUnAssigned>("SP_UNASIGNED_AUDITEE");

            return list;
        }

        public async void AuditorSave(AuditorAuditee data)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            System.Diagnostics.Debug.WriteLine(data);
            DBAccess dbAccess = new DBAccess(_options);
            if (data.ID.HasValue && data.ID != 0)
            {
               
                SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.VarChar, 10, data.ID.Value.ToString()),
                new SqlParameter("@npp", System.Data.SqlDbType.VarChar, 50, data.NPP),
                new SqlParameter("@kode", System.Data.SqlDbType.VarChar, 50, data.KODE),
                new SqlParameter("@prodi", System.Data.SqlDbType.VarChar, 50, data.PRODI),
                new SqlParameter("@email", System.Data.SqlDbType.VarChar, 50, data.EMAIL)
            };
                await dbAccess.ExecuteQuery("SP_AUDITOR_UPDATE", parameters);
            }
            else
            {
                SqlParameter[] parameters = {
                new SqlParameter("@npp", System.Data.SqlDbType.VarChar, 50, data.NPP),
                new SqlParameter("@kode", System.Data.SqlDbType.VarChar, 50, data.KODE),
                new SqlParameter("@prodi", System.Data.SqlDbType.VarChar, 50, data.PRODI),
                new SqlParameter("@email", System.Data.SqlDbType.VarChar, 50, data.EMAIL)
            };

                await dbAccess.ExecuteQuery("SP_AUDITOR_SAVE", parameters);
            }


        }

        public async void AuditorDelete(AuditorAuditee data)
        {
            DBOutput output = new DBOutput();
            output.status = true;

            DBAccess dbAccess = new DBAccess(_options);
            SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.VarChar, 10, data.ID.Value.ToString()),
            };

            await dbAccess.ExecuteQuery("SP_AUDITOR_DELETE", parameters);

        }

        public async void AuditeeDelete(AuditorAuditee data)
        {
            DBOutput output = new DBOutput();
            output.status = true;

            DBAccess dbAccess = new DBAccess(_options);
            SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.VarChar, 10, data.ID.Value.ToString()),
            };

            await dbAccess.ExecuteQuery("SP_AUDITEE_DELETE", parameters);

        }

        public async void AuditeeSave(AuditorAuditee data)
        {
            DBOutput output = new DBOutput();
            output.status = true;

            DBAccess dbAccess = new DBAccess(_options);
            if (data.ID.HasValue)
            {

                SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.VarChar, 10, data.ID.Value.ToString()),
                new SqlParameter("@id_unit", System.Data.SqlDbType.VarChar, 10, data.ID_UNIT.Value.ToString()),
                new SqlParameter("@singkatan", System.Data.SqlDbType.VarChar, 50, data.SINGKATAN),
                new SqlParameter("@nama", System.Data.SqlDbType.VarChar, 50, data.NAMA)
            };
                await dbAccess.ExecuteQuery("SP_AUDITEE_UPDATE", parameters);
            }
            else
            {
                SqlParameter[] parameters = {
                new SqlParameter("@id_unit", System.Data.SqlDbType.VarChar, 10, data.ID_UNIT.Value.ToString()),
                new SqlParameter("@singkatan", System.Data.SqlDbType.VarChar, 50, data.SINGKATAN),
                new SqlParameter("@nama", System.Data.SqlDbType.VarChar, 50, data.NAMA)
            };

                await dbAccess.ExecuteQuery("SP_AUDITEE_SAVE", parameters);
            }


        }
    }
}
