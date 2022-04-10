using System;

namespace EAudit.Models
{
    public class VerifikasiAuditor
    {
        public int? ID_VERIFIKASI { get; set; }
        public string KONFIRMASI { get; set; }
        public string CATATAN { get; set; }
        public string JENIS { get; set; }
        public string URAIAN { get; set; }
        public int ID_TANGGAPAN { get; set; }
        public string ID_TANGGAPAN_OLD { get; set; }
        public string NAMA_AUDITOR { get; set; }
        public string NAMA_UNIT { get; set; }
        public string URAIAN_TEMUAN { get; set; }
        public string JENIS_TEMUAN { get; set; }
        public string NO_STANDAR { get; set; }
        public string PERNYATAAN { get; set; }
        public string UNSUR_MANAJEMEN { get; set; }
        public string ANALISIS { get; set; }
        public string KOREKSI { get; set; }
        public string KOREKTIF { get; set; }
        public DateTime? TGL_TANGGAPAN { get; set; }
        public DateTime? TGL_KIRIM_TEMUAN { get; set; }
    }
}
