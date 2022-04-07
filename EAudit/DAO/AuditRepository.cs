using EAudit.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace EAudit.DAO
{
    public class AuditRepository : AuditInterface
    {
        public IOptions<AppSettings> _options;
        public AuditRepository(IOptions<AppSettings> options)
        {
            _options = options;
        }

        // Dashboard
        public async Task<List<Dashboard>> DashboardList()
        {
            ArrayList listParameters = new ArrayList();
            string query = @"SELECT d.*, CONVERT(varchar, TANGGAL, 110) as TANGGAL FROM dbo.TBL_DASHBOARD d";
            DBOutput output = new DBOutput();
            SqlParameter[] parameters = listParameters.ToArray(typeof(SqlParameter)) as SqlParameter[];
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);
            List<Dashboard> list = await dbAccess.ExecuteAsync<Dashboard>(query);
            return list;
        }
        public async void DashboardSave(string id_edit, string judul, string tanggal, string keterangan)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);
            string query = "";
            ArrayList listParameters = new ArrayList();
            if (!string.IsNullOrEmpty(id_edit))
            {
                query = @"UPDATE dbo.TBL_DASHBOARD
                            SET JUDUL=@judul, TANGGAL=@tanggal, KETERANGAN=@keterangan
                            WHERE ID_DASHBOARD=@id";
                listParameters.Add(new SqlParameter("@id", System.Data.SqlDbType.VarChar, 50, id_edit));
            }
            else
            {
                query = @"DECLARE @id int;
                                    SELECT @id = MAX(ID_DASHBOARD) + 1 FROM dbo.TBL_DASHBOARD WITH(UPDLOCK, HOLDLOCK);

                                    if (@id is null)
                                    begin
                                        set @id = 1
                                    end

                                    INSERT INTO dbo.TBL_DASHBOARD
                                    (ID_DASHBOARD, JUDUL, TANGGAL, KETERANGAN)
                                    VALUES(@id, @judul, @tanggal, @keterangan)";
            }
            listParameters.Add(new SqlParameter("@judul", System.Data.SqlDbType.VarChar, 50, judul.ToString()));
            listParameters.Add(new SqlParameter("@tanggal", System.Data.SqlDbType.VarChar, 50, tanggal.ToString()));
            listParameters.Add(new SqlParameter("@keterangan", System.Data.SqlDbType.Text, 50, keterangan.ToString()));
            SqlParameter[] parameters = listParameters.ToArray(typeof(SqlParameter)) as SqlParameter[];
            await dbAccess.Execute(query, parameters);
        }

        public async void DashboardDelete(string id)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);

            string query = @"DELETE FROM dbo.TBL_DASHBOARD
                                    WHERE ID_DASHBOARD = @id";

            SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.Int, 11, id),
            };
            await dbAccess.Execute(query, parameters);
        }
        public async Task<Dashboard> DashboardRow(Dashboard filter)
        {
            DBOutput output = new DBOutput();
            output.status = true;

            string query = @"SELECT * FROM dbo.TBL_DASHBOARD
                            WHERE ID_DASHBOARD = @id";

            SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.Int,10, filter.ID_DASHBOARD.Value.ToString())
            };

            DBAccess dbAccess = new DBAccess(_options);
            List<Dashboard> list = await dbAccess.ExecuteAsync<Dashboard>(query, parameters);

            return list[0];
        }

        //Logs





        //SECTION TEMUAN AUDIT
        // MENAMPILKAN TEMUAN AUDIT
        public async Task<List<AuditTemuan>> getTemuanAuditList(string search, string role, string id_auditor, string id_auditee)
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
            else if (role == "Auditee")
            {
                query += @" AND t.ID_AUDITEE=@id_auditee";
                listParameters.Add(new SqlParameter("@id_auditee", System.Data.SqlDbType.VarChar, 50, id_auditee));
            }
            query += @" AND (r.NPP LIKE '%' + COALESCE(@search,'%') + '%' OR
                                    k.NAMA_LENGKAP_GELAR LIKE '%' + COALESCE(@search, '%') + '%' OR
                                    t.URAIAN LIKE '%' + COALESCE(@search, '%') + '%')";
            listParameters.Add(new SqlParameter("@search", System.Data.SqlDbType.VarChar, 50, search));
            DBOutput output = new DBOutput();
            SqlParameter[] parameters = listParameters.ToArray(typeof(SqlParameter)) as SqlParameter[];
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);
            List<AuditTemuan> list = await dbAccess.ExecuteAsync<AuditTemuan>(query, parameters);
            return list;
        }


        // MENYIMPAN TEMUAN AUDIT
        public async void temuanSave(string id_edit, string id_auditor, string id_auditee, string jenis, string uraian, string standar)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);
            string query = "";
            ArrayList listParameters = new ArrayList();
            if (!string.IsNullOrEmpty(id_edit))
            {
                query = @"UPDATE dbo.TBL_TEMUAN
                            SET ID_AUDITEE=@id_auditee, JENIS=@jenis, URAIAN=@uraian, ID_STANDAR=@nomor
                            WHERE ID_TEMUAN=@id";
                listParameters.Add(new SqlParameter("@id", System.Data.SqlDbType.VarChar, 50, id_edit));
            }
            else
            {
                query = @"DECLARE @id int;
                                    SELECT @id = MAX(ID_TEMUAN) + 1 FROM dbo.TBL_TEMUAN WITH(UPDLOCK, HOLDLOCK);

                                    if(@id is null)
                                    begin
                                        set @id=1
                                    end
                                    INSERT INTO dbo.TBL_TEMUAN
                                    (ID_TEMUAN, ID_AUDITOR, ID_AUDITEE, JENIS, URAIAN, ID_STANDAR)
                                    VALUES(@id, @id_auditor, @id_auditee, @jenis, @uraian, @nomor)";
                listParameters.Add(new SqlParameter("@id_auditor", System.Data.SqlDbType.VarChar, 50, id_auditor.ToString()));
            }
            listParameters.Add(new SqlParameter("@id_auditee", System.Data.SqlDbType.VarChar, 50, id_auditee.ToString()));
            listParameters.Add(new SqlParameter("@jenis", System.Data.SqlDbType.VarChar, 50, jenis.ToString()));
            listParameters.Add(new SqlParameter("@uraian", System.Data.SqlDbType.Text, 50, uraian.ToString()));
            listParameters.Add(new SqlParameter("@nomor", System.Data.SqlDbType.VarChar, 50, standar.ToString()));
            SqlParameter[] parameters = listParameters.ToArray(typeof(SqlParameter)) as SqlParameter[];
            await dbAccess.Execute(query, parameters);
        }

        // HAPUS TEMUAN AUDIT
        public async void temuanDelete(string id_temuan)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);

            string query = @"DELETE FROM dbo.TBL_TEMUAN
                                    WHERE ID_TEMUAN = @id";

            SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.Int, 11, id_temuan),
            };
            await dbAccess.Execute(query, parameters);
        }


        // KIRIM TEMUAN AUDIT
        public async void temuanKirim(string id_temuan)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);

            string query = @"UPDATE TBL_TEMUAN SET SENT=1, SENTDATE=@date WHERE ID_TEMUAN=@idTemuan";

            SqlParameter[] parameters = {
                new SqlParameter("@date", System.Data.SqlDbType.VarChar, 200, DateTime.Now.ToString()),
                new SqlParameter("@idTemuan", System.Data.SqlDbType.Int, 11, id_temuan),
            };
            await dbAccess.Execute(query, parameters);
        }

        // DETAIL TEMUAN AUDIT
        public async Task<AuditTemuan> TemuanAuditRow(AuditTemuan filter)
        {
            DBOutput output = new DBOutput();
            output.status = true;

            string query = @"SELECT * FROM dbo.TBL_TEMUAN
                            WHERE ID_TEMUAN = @id";

            SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.Int,10, filter.ID_TEMUAN.Value.ToString())
            };

            DBAccess dbAccess = new DBAccess(_options);
            List<AuditTemuan> list = await dbAccess.ExecuteAsync<AuditTemuan>(query, parameters);

            return list[0];
        }

        // LIst Unsur Manajemen/Akar Masalah
        public async Task<List<UnsurManajemen>> UnsurManajemenList(string search)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            SqlParameter[] parameters = {
                new SqlParameter("@search", System.Data.SqlDbType.VarChar, 50, search)
            };

            DBAccess dbAccess = new DBAccess(_options);
            List<UnsurManajemen> list = await dbAccess.ExecuteReaderAsync<UnsurManajemen>("SP_UNSUR_MANAJEMEN_LIST", parameters);

            return list;
        }


        // MENYIMPAN TANGGAPAN AUDIT
        public async void TanggapanSave(string id_edit, string jenis, string akarMasalah, string analisis,
            string koreksi, string korektif, string tipe, string dokumen, string link, string nama, string idTemuan)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);
            string query = "";
            ArrayList listParameters = new ArrayList();
            if (jenis == "vid")
            {
                tipe = "-";
                dokumen = null;
            }
            else
            {
                link = null;
            }
            if (!string.IsNullOrEmpty(id_edit))
            {
                var tambahan = "";
                if (jenis == "doc")
                {
                    if (dokumen == null || dokumen == "")
                    {

                    }
                    else
                    {
                        tambahan = " DOKUMEN=@UPLOAD,";
                        listParameters.Add(new SqlParameter("@UPLOAD", System.Data.SqlDbType.VarChar, 50, dokumen));
                    }
                }
                query = @"UPDATE dbo.TBL_TANGGAPAN
                            SET ID_UNSUR_MANAJEMEN=@UNSUR, ANALISIS=@ANALISIS, KOREKSI=@KOREKSI, KOREKTIF=@KOREKTIF,
                            TIPE_FILE=@TIPE," + tambahan + " LINK=@LINK, NAMA_FILE=@NAMA,ID_TEMUAN=@idTemuan WHERE ID_TANGGAPAN=@id";
                listParameters.Add(new SqlParameter("@id", System.Data.SqlDbType.VarChar, 50, id_edit));

            }
            else
            {
                query = @"DECLARE @id int;
                        SELECT @id = MAX(ID_TANGGAPAN) + 1 FROM dbo.TBL_TANGGAPAN WITH(UPDLOCK, HOLDLOCK);
                        if(@id is null)
                        begin
                            set @id=1
                        end

                        INSERT INTO dbo.TBL_TANGGAPAN(ID_TANGGAPAN, ID_UNSUR_MANAJEMEN, ANALISIS, KOREKSI, KOREKTIF, 
                        TIPE_FILE,DOKUMEN, LINK, NAMA_FILE, ID_TEMUAN)
                        VALUES(@id, @UNSUR, @ANALISIS, @KOREKSI, @KOREKTIF, @TIPE,  @UPLOAD, @LINK, @NAMA, @idTemuan);

                        UPDATE TBL_TEMUAN SET STATUSTANGGAPAN=@STATUS, TANGGALTANGGAPAN=@TANGGAL 
                        WHERE ID_TEMUAN=@idTemuan";
                listParameters.Add(new SqlParameter("@STATUS", System.Data.SqlDbType.VarChar, 200, "true"));
                listParameters.Add(new SqlParameter("@TANGGAL", System.Data.SqlDbType.VarChar, 200, DateTime.Now.ToString()));
                listParameters.Add(new SqlParameter("@UPLOAD", System.Data.SqlDbType.VarChar, 50, dokumen));
            }
            listParameters.Add(new SqlParameter("@idTemuan", System.Data.SqlDbType.VarChar, 50, idTemuan));
            listParameters.Add(new SqlParameter("@UNSUR", System.Data.SqlDbType.VarChar, 50, akarMasalah));
            listParameters.Add(new SqlParameter("@ANALISIS", System.Data.SqlDbType.Text, 255, analisis));
            listParameters.Add(new SqlParameter("@KOREKSI", System.Data.SqlDbType.Text, 255, koreksi));
            listParameters.Add(new SqlParameter("@KOREKTIF", System.Data.SqlDbType.Text, 255, korektif));
            listParameters.Add(new SqlParameter("@TIPE", System.Data.SqlDbType.VarChar, 50, tipe));
            listParameters.Add(new SqlParameter("@LINK", System.Data.SqlDbType.VarChar, 200, link));
            listParameters.Add(new SqlParameter("@NAMA", System.Data.SqlDbType.VarChar, 200, nama));

            SqlParameter[] parameters = listParameters.ToArray(typeof(SqlParameter)) as SqlParameter[];
            await dbAccess.Execute(query, parameters);
        }



        public async Task<List<TanggapanAudit>> TanggapanList(string search, string role, string id_auditor, string id_auditee)
        {
            ArrayList listParameters = new ArrayList();


            string query = "";
            if (role == "Admin")
            {
                query = @"SELECT * FROM VIEWTANGGAPANVERIFIKASI WHERE 1=1 ";
            }
            else if (role == "Auditor")
            {
                query = @"SELECT t.*,NULL as NAMA_AUDITOR, um.DESKRIPSI, tva.KONFIRMASI,tm.ID_AUDITOR,tm.ID_AUDITEE FROM dbo.TBL_TANGGAPAN t
                        JOIN dbo.TBL_TEMUAN as tm ON tm.ID_TEMUAN=t.ID_TEMUAN
                        JOIN dbo.TBL_UNSUR_MANAJEMEN um ON t.ID_UNSUR_MANAJEMEN = um.ID_UNSUR_MANAJEMEN
                        LEFT OUTER JOIN TBL_VERIFIKASI_AUDITOR tva ON t.ID_TANGGAPAN = tva.ID_TANGGAPAN
                        WHERE t.SENT='1' AND tva.KONFIRMASI IS NULL ";
                query += @" AND ID_AUDITOR=@id_auditor";
                listParameters.Add(new SqlParameter("@id_auditor", System.Data.SqlDbType.VarChar, 50, id_auditor));
            }
            else if (role == "Auditee")
            {
                query = @"SELECT * FROM VIEWTANGGAPANVERIFIKASI WHERE 1=1 ";
                query += @" AND ID_AUDITEE=@id_auditee";
                listParameters.Add(new SqlParameter("@id_auditee", System.Data.SqlDbType.VarChar, 50, id_auditee));
            }
            query += @" AND (DESKRIPSI LIKE '%' + COALESCE(@search,'%') + '%' OR
                                    ANALISIS LIKE '%' + COALESCE(@search, '%') + '%' OR
                                    KOREKSI LIKE '%' + COALESCE(@search, '%') + '%')";
            listParameters.Add(new SqlParameter("@search", System.Data.SqlDbType.VarChar, 50, search));
            DBOutput output = new DBOutput();
            SqlParameter[] parameters = listParameters.ToArray(typeof(SqlParameter)) as SqlParameter[];
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);
            List<TanggapanAudit> list = await dbAccess.ExecuteAsync<TanggapanAudit>(query, parameters);
            return list;
        }

        public async Task<TanggapanAudit> TanggapanAuditRow(TanggapanAudit filter)
        {
            DBOutput output = new DBOutput();
            output.status = true;

            string query = @"SELECT * FROM dbo.TBL_TANGGAPAN
                                    WHERE ID_TANGGAPAN = @id";

            SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.Int,10, filter.ID_TANGGAPAN.Value.ToString())
            };

            DBAccess dbAccess = new DBAccess(_options);
            List<TanggapanAudit> list = await dbAccess.ExecuteAsync<TanggapanAudit>(query, parameters);

            return list[0];
        }

        public async void TanggapanDelete(string id_tanggapan)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);

            string query = @"DELETE FROM dbo.TBL_TANGGAPAN
                                    WHERE ID_TANGGAPAN = @id";

            SqlParameter[] parameters = {
                new SqlParameter("@id", System.Data.SqlDbType.Int, 11, id_tanggapan),
            };
            await dbAccess.Execute(query, parameters);
        }

        // Kirim Tanggapan Oleh Auditee
        public async Task<TanggapanAudit> TanggapanKirim(string id_tanggapan)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);

            string query = @"UPDATE TBL_TANGGAPAN SET SENT=1, SENTDATE=@date WHERE ID_TANGGAPAN=@idTanggapan";
            SqlParameter[] parameters = {
                new SqlParameter("@idTanggapan", System.Data.SqlDbType.VarChar,10, id_tanggapan),
                new SqlParameter("@date", System.Data.SqlDbType.VarChar,200, DateTime.Now.ToString())
            };
            await dbAccess.Execute(query, parameters);

            string query2 = @"UPDATE TBL_VERIFIKASI_AUDITOR SET ID_TANGGAPAN=NULL, ID_TANGGAPAN_OLD=@idTanggapan WHERE ID_TANGGAPAN=@idTanggapan";
            SqlParameter[] parameters2 = {
                new SqlParameter("@idTanggapan", System.Data.SqlDbType.VarChar,10, id_tanggapan),
            };
            await dbAccess.Execute(query2, parameters2);

            string query_tampil = @"SELECT * FROM VTANGGAPAN WHERE ID_TANGGAPAN=@idtanggapan";
            SqlParameter[] parameters_tampil = {
                new SqlParameter("@idtanggapan", System.Data.SqlDbType.Int,10, id_tanggapan)
            };
            List<TanggapanAudit> list = await dbAccess.ExecuteAsync<TanggapanAudit>(query_tampil, parameters_tampil);

            return list[0];
        }

        //Selesaikan Tanggapan By Admin
        public async void TanggapanSelesaikan(string id_tanggapan)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);

            string query = @"UPDATE TBL_TANGGAPAN SET FINISHBYADMIN=1, FINISHDATE=@date WHERE ID_TANGGAPAN=@idTanggapan";

            SqlParameter[] parameters = {
                new SqlParameter("@idTanggapan", System.Data.SqlDbType.VarChar,10, id_tanggapan),
                new SqlParameter("@date", System.Data.SqlDbType.VarChar,200, DateTime.Now.ToString())
            };
            await dbAccess.Execute(query, parameters);
        }
        public async void TanggapanVerifikasi(string id_tanggapan, string konfirmasi, string catatan, string uraian)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);
            ArrayList listParameters = new ArrayList();

            string query = @"DECLARE @id int;
                    SELECT @id = MAX(ID_VERIFIKASI) + 1 FROM dbo.TBL_VERIFIKASI_AUDITOR WITH(UPDLOCK, HOLDLOCK);

                    if(@id is null)
                    begin
                        set @id=1
                    end

                    INSERT INTO dbo.TBL_VERIFIKASI_AUDITOR
                    (ID_VERIFIKASI, KONFIRMASI, CATATAN, URAIAN, ID_TANGGAPAN)
                    VALUES(@id, @konfirmasi, @catatan, @uraian, @id_tanggapan);

                    IF (@konfirmasi <> 'Setuju') BEGIN
                    UPDATE TBL_TANGGAPAN SET SENTDATE=NULL, SENT=0 WHERE ID_TANGGAPAN = @id_tanggapan;
                    END";
            listParameters.Add(new SqlParameter("@konfirmasi", System.Data.SqlDbType.VarChar, 50, konfirmasi));
            listParameters.Add(new SqlParameter("@catatan", System.Data.SqlDbType.Text, 200, catatan));
            listParameters.Add(new SqlParameter("@uraian", System.Data.SqlDbType.Text, 255, uraian));
            listParameters.Add(new SqlParameter("@id_tanggapan", System.Data.SqlDbType.VarChar, 50, id_tanggapan));

            SqlParameter[] parameters = listParameters.ToArray(typeof(SqlParameter)) as SqlParameter[];
            await dbAccess.Execute(query, parameters);
        }

        //Verifikasi Auditor
        // Menampilkan List Verifikasi Auditor
        public async Task<List<VerifikasiAuditor>> VerifikasiAuditorList(string search, string role, string id_auditor, string id_auditee)
        {
            ArrayList listParameters = new ArrayList();


            string query = @"SELECT tva.*,tm.ID_AUDITOR,tm.ID_AUDITEE FROM dbo.TBL_VERIFIKASI_AUDITOR tva LEFT JOIN dbo.TBL_TANGGAPAN tgp ON tva.ID_TANGGAPAN=tgp.ID_TANGGAPAN
                             LEFT JOIN dbo.TBL_TEMUAN tm ON tm.ID_TEMUAN=tgp.ID_TEMUAN WHERE 1=1 ";
            if (role == "Auditor")
            {
                query += @" AND ID_AUDITOR=@id_auditor";
                listParameters.Add(new SqlParameter("@id_auditor", System.Data.SqlDbType.VarChar, 50, id_auditor));
            }
            else if (role == "Auditee")
            {
                query += @" AND ID_AUDITEE=@id_auditee";
                listParameters.Add(new SqlParameter("@id_auditee", System.Data.SqlDbType.VarChar, 50, id_auditee));
            }
            query += @" AND (tva.KONFIRMASI LIKE '%' + COALESCE(@search,'%') + '%' OR
                                    tva.CATATAN LIKE '%' + COALESCE(@search, '%') + '%' OR
                                    tva.URAIAN LIKE '%' + COALESCE(@search, '%') + '%')";
            listParameters.Add(new SqlParameter("@search", System.Data.SqlDbType.VarChar, 50, search));
            DBOutput output = new DBOutput();
            SqlParameter[] parameters = listParameters.ToArray(typeof(SqlParameter)) as SqlParameter[];
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);
            List<VerifikasiAuditor> list = await dbAccess.ExecuteAsync<VerifikasiAuditor>(query, parameters);
            return list;
        }


        // Logs
        public async Task<List<Log>> LogList(string search)
        {
            ArrayList listParameters = new ArrayList();

            string query = @"select * from dbo.TBL_LOG WHERE 1=1 ";
            query += @" AND (TANGGAL_WAKTU LIKE '%' + COALESCE(@search,'%') + '%' OR
                                    KETERANGAN LIKE '%' + COALESCE(@search, '%') + '%' OR
                                    NPP LIKE '%' + COALESCE(@search, '%') + '%')";
            listParameters.Add(new SqlParameter("@search", System.Data.SqlDbType.VarChar, 50, search));
            DBOutput output = new DBOutput();
            SqlParameter[] parameters = listParameters.ToArray(typeof(SqlParameter)) as SqlParameter[];
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);
            List<Log> list = await dbAccess.ExecuteAsync<Log>(query, parameters);
            return list;
        }
        public async void LogSave(string npp, string keteranganLog)
        {
            DBOutput output = new DBOutput();
            output.status = true;
            DBAccess dbAccess = new DBAccess(_options);
            ArrayList listParameters = new ArrayList();
            string query = @"DECLARE @ID_LOG int;
                                    SELECT @ID_LOG = MAX(ID_LOG) + 1 FROM dbo.TBL_LOG WITH(UPDLOCK, HOLDLOCK);
                                    if (@ID_LOG is null)
                                    begin
                                        set @ID_LOG = 1
                                    end
                                    INSERT INTO dbo.TBL_LOG (ID_LOG, TANGGAL_WAKTU, KETERANGAN, NPP) 
                                    VALUES(@ID_LOG, @TANGGAL_WAKTU, @KETERANGAN, @NPP); 
                                    SELECT CAST(SCOPE_IDENTITY() as int)";
            listParameters.Add(new SqlParameter("@TANGGAL_WAKTU", System.Data.SqlDbType.VarChar, 50, DateTime.Now.ToString()));
            listParameters.Add(new SqlParameter("@KETERANGAN", System.Data.SqlDbType.Text, 200, keteranganLog.ToString()));
            SqlParameter[] parameters = listParameters.ToArray(typeof(SqlParameter)) as SqlParameter[];
            await dbAccess.Execute(query, parameters);
        }
    }
}
