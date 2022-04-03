using EAudit.Models;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace EAudit.DAO
{
    public class EAuditRepository : IEAuditInterface
    {
        /// <summary>
        /// Dependency injection
        /// </summary>
        public IOptions<AppSettings> _options;
        
        /// <summary>
        /// Class Constructor of... 
        /// </summary>
        /// <param name="options"></param>
        public EAuditRepository(IOptions<AppSettings> options)
        {
            _options = options;
        }

        /// <summary>
        /// Menampilkan semua jadwal kegiatan / penugasan
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<List<Audit_JadwalKegiatan>> getAllJadwalKegiatanList(string search)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            SqlParameter[] parameters = {
                new SqlParameter("@search", System.Data.SqlDbType.VarChar, 50, search)
            };

            DBAccess dbAccess = new DBAccess(_options);
            List<Audit_JadwalKegiatan> list = await dbAccess.ExecuteReaderAsync<Audit_JadwalKegiatan>("SP_JADWALKEGIATAN_LIST", parameters);

            return list;
        }
        /// <summary>
        /// Menampilkan jadwal yang dipilih
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Audit_JadwalKegiatan> getAllJadwalKegiatanRow(int id)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            SqlParameter[] parameters = {
                new SqlParameter("@idJadwal", System.Data.SqlDbType.Int, 50, id.ToString())
            };

            DBAccess dbAccess = new DBAccess(_options);
            List<Audit_JadwalKegiatan> list = await dbAccess.ExecuteReaderAsync<Audit_JadwalKegiatan>("SP_JADWALKEGIATAN_ROW", parameters);

            return list[0];
        }
        /// <summary>
        /// Simpan Jadwal Kegiatan
        /// </summary>
        /// <param name="data"></param>
        public async void JadwalKegiatanSave(Audit_JadwalKegiatan data)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            System.Diagnostics.Debug.WriteLine(data);
            DBAccess dbAccess = new DBAccess(_options);
            if (data.ID.HasValue && data.ID != null)
            {

                SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.VarChar, 10, data.ID.Value.ToString()),
                new SqlParameter("@tanggal", System.Data.SqlDbType.VarChar, 10, data.TANGGAL),
                 new SqlParameter("@idAuditee", System.Data.SqlDbType.VarChar, 10, data.ID_AUDITEE.Value.ToString()),
                new SqlParameter("@start", System.Data.SqlDbType.VarChar, 50, data.WS),
                new SqlParameter("@end", System.Data.SqlDbType.VarChar, 50, data.WE),
                new SqlParameter("@auditor", System.Data.SqlDbType.VarChar, 50, data.ID_AUDITOR)
            };
                await dbAccess.ExecuteQuery("SP_UPDATE_JADWAL_KEGIATAN", parameters);
            }
            else
            {
                SqlParameter[] parameters = {
                new SqlParameter("@tanggal", System.Data.SqlDbType.VarChar, 10, data.TANGGAL),
                new SqlParameter("@idAuditee", System.Data.SqlDbType.VarChar, 10, data.ID_AUDITEE.Value.ToString()),
                new SqlParameter("@start", System.Data.SqlDbType.VarChar, 50, data.WS),
                new SqlParameter("@end", System.Data.SqlDbType.VarChar, 50, data.WE),
                new SqlParameter("@auditor", System.Data.SqlDbType.VarChar, 50, data.ID_AUDITOR)
            };

                await dbAccess.ExecuteQuery("SP_JADWALKEGIATAN_SAVE", parameters);
            }


        }


        public async void HapusJadwal(Audit_JadwalKegiatan data)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);
            string query = @"DELETE FROM dbo.TBL_JADWAL_KEGIATAN  WHERE ID_JADWAL = @id";

            SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.VarChar, 10, data.ID.Value.ToString()),
            };
            await dbAccess.Execute(query, parameters);
        }


        public async Task<List<AuditTemuan>> getAllTemuan(string search, string role, string id_auditor)
        {
            ArrayList listParameters = new ArrayList();

            string query = @"SELECT t.*, r.NPP, k.NAMA_LENGKAP_GELAR, r.KODE, e.ID_UNIT, u.NAMA_UNIT, e.SINGKATAN,
                                    s.ID_STANDAR, s.NO_STANDAR, s.PERNYATAAN FROM dbo.TBL_TEMUAN t
                                    JOIN dbo.TBL_STANDAR_SPMI s ON t.ID_STANDAR = s.ID_STANDAR
                                    JOIN dbo.TBL_AUDITEE e ON t.ID_AUDITEE = e.ID_AUDITEE                                    
                                    JOIN dbo.TBL_AUDITOR r ON t.ID_AUDITOR = r.ID_AUDITOR
                                    JOIN siatmax.MST_UNIT u ON e.ID_UNIT = u.ID_UNIT
                                    JOIN simka.MST_KARYAWAN k ON r.NPP = k.NPP 
                                    WHERE 1=1 ";
            
            if (role == "Auditor")
            {
                query += @" AND t.ID_AUDITOR=@id_auditor";
                listParameters.Add(new SqlParameter("@id_auditor", System.Data.SqlDbType.VarChar, 50, id_auditor));
            }
            query += @" AND (r.NPP LIKE '%' + COALESCE(@search,'%') + '%' OR
                                    k.NAMA_LENGKAP_GELAR LIKE '%' + COALESCE(@search, '%') + '%' OR
                                    t.URAIAN LIKE '%' + COALESCE(@search, '%') + '%')";
            listParameters.Add(new SqlParameter("@search", System.Data.SqlDbType.VarChar, 50, search));

            DBOutput output = new DBOutput();
            SqlParameter[] parameters = listParameters.ToArray(typeof(SqlParameter)) as SqlParameter[];
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);
            List<AuditTemuan> list = await dbAccess.ExecuteAsync<AuditTemuan>(query,parameters);

            return list;
        }

        public async void temuanSave(string id_auditor, string id_auditee, string jenis, string uraian, string standar)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);

            string query = @"DECLARE @id int;
                                    SELECT @id = MAX(ID_TEMUAN) + 1 FROM dbo.TBL_TEMUAN WITH(UPDLOCK, HOLDLOCK);

                                    if(@id is null)
                                    begin
                                        set @id=1
                                    end

                                    INSERT INTO dbo.TBL_TEMUAN
                                    (ID_TEMUAN, ID_AUDITOR, ID_AUDITEE, JENIS, URAIAN, ID_STANDAR)
                                    VALUES(@id, @id_auditor, @id_auditee, @jenis, @uraian, @nomor)";

            SqlParameter[] parameters = {
                new SqlParameter("@id_auditor", System.Data.SqlDbType.VarChar, 50, id_auditor.ToString()),
                new SqlParameter("@id_auditee",System.Data.SqlDbType.VarChar, 50, id_auditee.ToString()),
                new SqlParameter("@jenis",System.Data.SqlDbType.VarChar, 50, jenis.ToString()),
                new SqlParameter("@uraian",System.Data.SqlDbType.Text, 50, uraian.ToString()),
                new SqlParameter("@nomor",System.Data.SqlDbType.VarChar, 50, standar.ToString()),
            };
            await dbAccess.Execute(query, parameters);
        }
    }
}
