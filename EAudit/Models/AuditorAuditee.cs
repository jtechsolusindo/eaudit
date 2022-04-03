using System;

namespace EAudit.Models
{
    public class AuditorAuditee
    {
        public Int32? ID { get; set; }
        public int? ID_JADWAL { get; set; }
        public int? ID_UNIT { get; set; }
        public string NPP { get; set; }
        public string NAMA { get; set; }
        public string SINGKATAN { get; set; }
        public string PRODI { get; set; }
        public string EMAIL { get; set; }
        public string NAMA_UNIT { get; set; }
        public string KODE_UNIT { get; set; }
        public string KODE { get; set; }
        public string NAMA_LENGKAP_GELAR { get; set; }
        public string mode { get; set; }
    }
}
