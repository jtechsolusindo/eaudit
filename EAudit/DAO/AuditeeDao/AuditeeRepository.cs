using EAudit.Models;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace EAudit.DAO.AuditeeDao
{
    public class AuditeeRepository : IAuditee
    {
        public IOptions<AppSettings> _options;

        public AuditeeRepository(IOptions<AppSettings> options)
        {
            _options = options;
        }
        public async Task<List<Auditee>> getAuditeeList(string search)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);
            ArrayList listParameters = new ArrayList();
            string query = @"SELECT a.ID_AUDITEE AS ID, k.NPP, a.ID_JADWAL, a.ID_UNIT, a.SINGKATAN AS KODE, NULL AS PRODI, NULL AS EMAIL , u.NAMA_UNIT, u.KODE_UNIT, a.NAMA,k.NAMA_LENGKAP_GELAR
                            FROM dbo.TBL_AUDITEE a
                            JOIN siatmax.MST_UNIT u ON a.ID_UNIT = u.ID_UNIT
                            JOIN simka.MST_KARYAWAN k on u.NPP = k.NPP
                            WHERE 1=1 ";
            query += @" AND (a.NAMA LIKE '%' + COALESCE(@search,'%') + '%' OR
                    k.NAMA_LENGKAP_GELAR LIKE '%' + COALESCE(@search,'%') + '%' OR
                    u.NAMA_UNIT LIKE '%' + COALESCE(@search,'%') + '%' OR
                    a.NPP LIKE '%' + COALESCE(@search,'%') + '%')";
            listParameters.Add(new SqlParameter("@search", System.Data.SqlDbType.VarChar, 50, search));


            SqlParameter[] parameters = listParameters.ToArray(typeof(SqlParameter)) as SqlParameter[];
            List<Auditee> list = await dbAccess.ExecuteAsync<Auditee>(query, parameters);
            return list;
        }
    }
}
