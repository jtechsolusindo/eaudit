namespace EAudit.Models
{
    public class UserInfo
    {
        public int? ID_AUDITOR { get; set; }
        public int? ID_UNIT { get; set; }
        public int? ID_JADWAL { get; set; }
        public int? ID_AUDITEE { get; set; }
        public string KODE { get; set; }
        public string PRODI { get; set; }
        public string EMAIL { get; set; }
        public string NPP { get; set; }
        public string NAMA { get; set; }
        public string PASSWORD { get; set; }
        public string ROLE { get; set; }
    }
}
