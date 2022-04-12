using EAudit.DAO.AuditorDao;
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
        private IAuditor _auditorRepository;
        public IOptions<AppSettings> _options;

        public AuditeeRepository(IOptions<AppSettings> options, IAuditor auditorRepository)
        {
            _options = options;
            _auditorRepository = auditorRepository;
        }

        

        public async Task<List<Auditee>> AuditeeList(string search, string role, string id_auditor, string id_auditee)
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
            
            if (role == "Auditor")
            {
                //Auditor filter = new Auditor();
                //filter.ID = int.Parse(id_auditor);
                //Auditor data = await _auditorRepository.AuditorRow(filter);
                //query  += @" AND a.ID_UNIT=" + data.ID_UNIT;

                //Auditee filter = new Auditee();
                //filter.ID = int.Parse(id_auditee);
                //Auditor data = await _auditorRepository.AuditorRow(filter);
                //query += @" AND a.ID_UNIT=" + data.ID_UNIT;

                query += @" AND a.ID_UNIT IN(SELECT b.ID_UNIT FROM TBL_JADWAL_KEGIATAN a 
                            JOIN TBL_AUDITEE b ON a.ID_AUDITEE=b.ID_AUDITEE 
                            WHERE (SELECT COUNT(splited_data) FROM dbo.SPLIT_STRING(a.ID_AUDITOR_TEXT,'#') 
                            WHERE splited_data='"+id_auditor+ "')=1 ORDER BY a.ID_JADWAL DESC OFFSET 0 ROWS) ";
            }
            query += @" AND (a.NAMA LIKE '%' + COALESCE(@search,'%') + '%' OR
                    k.NAMA_LENGKAP_GELAR LIKE '%' + COALESCE(@search,'%') + '%' OR
                    u.NAMA_UNIT LIKE '%' + COALESCE(@search,'%') + '%' OR
                    a.NPP LIKE '%' + COALESCE(@search,'%') + '%')";
            listParameters.Add(new SqlParameter("@search", System.Data.SqlDbType.VarChar, 50, search));


            SqlParameter[] parameters = listParameters.ToArray(typeof(SqlParameter)) as SqlParameter[];
            List<Auditee> list = await dbAccess.ExecuteAsync<Auditee>(query, parameters);
            return list;
        }

        public async Task<Auditee> AuditeeRow(Auditee filter)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.Int,10, filter.ID.Value.ToString())
            };

            DBAccess dbAccess = new DBAccess(_options);
            List<Auditee> list = await dbAccess.ExecuteReaderAsync<Auditee>("SP_AUDITEE_ROW", parameters);

            return list[0];
        }

      

        public async void AuditeeSave(Auditee data)
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

        public async void AuditeeDelete(Auditee data)
        {
            DBOutput output = new DBOutput();
            output.status = true;

            DBAccess dbAccess = new DBAccess(_options);
            SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.VarChar, 10, data.ID.Value.ToString()),
            };

            await dbAccess.ExecuteQuery("SP_AUDITEE_DELETE", parameters);
        }
    }
}
