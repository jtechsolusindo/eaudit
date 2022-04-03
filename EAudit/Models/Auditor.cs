using Microsoft.AspNetCore.Mvc;

namespace EAudit.Models
{
    public class Auditor
    {
        public int? ID { get; set; }
        public string NPP { get; set; }
        public int? ID_JADWAL { get; set; }
        public int? ID_UNIT { get; set; }
        public string KODE { get; set; }
        public string PRODI { get; set; }
        public string EMAIL { get; set; }
        public string NAMA_UNIT { get; set; }
        public string KODE_UNIT { get; set; }
        public string NAMA_LENGKAP_GELAR { get; set; }
    }
}
