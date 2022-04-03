using System.ComponentModel.DataAnnotations;

namespace EAudit.ViewModels
{
    public class LoginAuthentication
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
