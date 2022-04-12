using System;

namespace EAudit.Models
{
    public class TanggapanAudit
    {
        public int? ID_TANGGAPAN { get; set; }
        public string DESKRIPSI { get; set; }
        public int ID_UNSUR_MANAJEMEN { get; set; }
        public string ANALISIS { get; set; }
        public string KOREKSI { get; set; }
        public string KOREKTIF { get; set; }
        public string TIPE_FILE { get; set; }

        public string LINK { get; set; }
        public string DOKUMEN { get; set; }
        public string NAMA_FILE { get; set; }
        public int ID_TEMUAN { get; set; }
        public Boolean SENT { get; set; }
        public DateTime? SENTDATE { get; set; }
        public DateTime? TANGGAL { get; set; }
        public string KONFIRMASI { get; set; }

        public Boolean? FINISHBYADMIN { get; set; }



        public string JENIS { get; set; }

        public DateTime? TANGGALTANGGAPAN { get; set; }
        public string EMAIL { get; set; }
        public string NAMA { get; set; }
        public string NAMA_AUDITOR { get; set; }

        public string NAMA_UNIT { get; set; }
    }
}
