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
    }
}
