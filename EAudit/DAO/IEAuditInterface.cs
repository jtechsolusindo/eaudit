using EAudit.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAudit.DAO
{
    public interface IEAuditInterface
    {
        #region Penugasan
        public Task<List<Audit_JadwalKegiatan>> getAllJadwalKegiatanList(string search);
        public Task<Audit_JadwalKegiatan> getAllJadwalKegiatanRow(int id);
        public void JadwalKegiatanSave(Audit_JadwalKegiatan data);
        void HapusJadwal(Audit_JadwalKegiatan data);
        #endregion

        public Task<List<AuditTemuan>> getAllTemuan(string search, string role,string id_auditor);

        public void temuanSave(string id_auditor, string id_auditee, string jenis, string uraian, string standar);

    }
}
