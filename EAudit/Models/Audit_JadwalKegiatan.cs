using System;
namespace EAudit.Models
{
    public class Audit_JadwalKegiatan
    {
        public int? ID { get; set; }
        public string TANGGAL { get; set; }
        public string WS { get; set; }
        public string WE { get; set; }
        public string WAWE { get; set; }
        public int? ID_AUDITEE { get; set; }
        public string ID_AUDITOR { get; set; }
        public int? ID_UNIT { get; set; }
        public string NAMA_UNIT { get; set; }
        public string NAMA_LENGKAP { get; set; }
        public string search { get; set; }
    }
}
