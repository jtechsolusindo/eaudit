using System;
namespace EAudit.Models
{
    public class Audit_Temuan
    {
        public int? ID_TEMUAN { get; set; }
        public int ID_AUDITOR { get; set; }
        public int ID_AUDITEE { get; set; }
        public string JENIS { get; set; }
        public string URAIAN { get; set; }
        public int ID_STANDAR { get; set; }
        public DateTime? SENTDATE { get; set; }
        public Boolean SENT { get; set; }
        public Boolean STATUSTANGGAPAN { get; set; }
        public DateTime? TANGGALTANGGAPAN { get; set; }
        public string EMAIL { get; set; }
        public string NPP { get; set; }
        public string NAMA_LENGKAP_GELAR { get; set; }
        public string KODE { get; set; }
        public int ID_UNIT { get; set; }
        public string NAMA_UNIT { get; set; }
        public string SINGKATAN { get; set; }

        public string NO_STANDAR { get; set; }
        public string PERNYATAAN { get; set; }
    }
}
