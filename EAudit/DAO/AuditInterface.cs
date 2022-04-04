using EAudit.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAudit.DAO
{
    public interface AuditInterface
    {
        // Dashboard
        public Task<List<Dashboard>> DashboardList();

        // Temuan Audit
        public Task<List<AuditTemuan>> getTemuanAuditList(string search, string role, string id_auditor, string id_auditee);
        public Task<AuditTemuan> TemuanAuditRow(AuditTemuan filter);
        public void temuanSave(string id_edit,string id_auditor, string id_auditee, string jenis, string uraian, string standar);

        public void temuanDelete(string id_temuan);

        public void temuanKirim(string id_temuan);


        // unsur manajemen
        public Task<List<UnsurManajemen>> UnsurManajemenList(string search);

        // Tanggapan
        public Task<List<TanggapanAudit>> TanggapanList(string search, string role, string id_auditor, string id_auditee);
        public void TanggapanSave(
            string id_edit, string jenis, string akarMasalah, string analisis,
            string koreksi, string korektif, string tipe, string dokumen, string link, string nama, string idTemuan);

        public Task<TanggapanAudit> TanggapanAuditRow(TanggapanAudit filter);

        public void TanggapanDelete(string id_tanggapan);

        public Task<TanggapanAudit> TanggapanKirim(string id_tanggapan);

        public void TanggapanVerifikasi(string id_tanggapan, string konfirmasi, string catatan, string uraian);

        public void TanggapanSelesaikan(string id_tanggapan);


        // verifikasi auditor
        public Task<List<VerifikasiAuditor>>VerifikasiAuditorList(string search, string role, string id_auditor, string id_auditee);

        // logs
        public Task<List<Log>> LogList(string search);
    }
}
