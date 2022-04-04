using EAudit.Models;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace EAudit.DAO.AuditorDao
{
    public class AuditorRepository : IAuditor
    {
        public IOptions<AppSettings> _options;

        public AuditorRepository(IOptions<AppSettings> options)
        {
            _options = options;
        }
        public async Task<List<Auditor>> AuditorList(string search)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);
            ArrayList listParameters = new ArrayList();
            string query = @"SELECT a.ID_AUDITOR AS ID, a.NPP, a.ID_JADWAL, u.ID_UNIT, a.KODE AS KODE, a.PRODI, a.EMAIL, u.NAMA_UNIT, u.KODE_UNIT, k.NAMA_LENGKAP_GELAR FROM dbo.TBL_AUDITOR a 
                                    JOIN simka.MST_KARYAWAN k ON a.NPP = k.NPP
                                    JOIN siatmax.MST_UNIT u ON k.ID_UNIT = u.ID_UNIT
                                    WHERE 1=1 ";
            query += @" AND ( a.PRODI LIKE '%' + COALESCE(@search,'%') + '%' OR
                        k.NAMA_LENGKAP_GELAR LIKE '%' + COALESCE(@search,'%') + '%' OR
                        u.NAMA_UNIT LIKE '%' + COALESCE(@search,'%') + '%' OR
                        a.EMAIL LIKE '%' + COALESCE(@search,'%') + '%' OR
                        a.NPP LIKE '%' + COALESCE(@search,'%') + '%')";
            listParameters.Add(new SqlParameter("@search", System.Data.SqlDbType.VarChar, 50, search));

           
            SqlParameter[] parameters = listParameters.ToArray(typeof(SqlParameter)) as SqlParameter[];
            List<Auditor> list = await dbAccess.ExecuteAsync<Auditor>(query, parameters);
            return list;
        }

        public async void AuditorSave(Auditor data)
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

        public async void AuditorDelete(Auditor data)
        {
            DBOutput output = new DBOutput();
            output.status = true;

            DBAccess dbAccess = new DBAccess(_options);
            SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.VarChar, 10, data.ID.Value.ToString()),
            };

            await dbAccess.ExecuteQuery("SP_AUDITOR_DELETE", parameters);

        }

        public async Task<Auditor> AuditorRow(Auditor filter)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.Int,10, filter.ID.Value.ToString())
            };

            DBAccess dbAccess = new DBAccess(_options);
            List<Auditor> list = await dbAccess.ExecuteReaderAsync<Auditor>("SP_AUDITOR_ROW", parameters);

            return list[0];
        }
    }
}
