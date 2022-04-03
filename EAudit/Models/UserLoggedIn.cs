namespace EAudit.Models
{
    public class UserLoggedIn
    {
        public string id_auditor { get; set; }
        public string id_auditee { get; set; }
        public string name{ get; set; }
        public string email { get; set; }
        public string npp { get; set; }
        public string role { get; set; }
    }
}
