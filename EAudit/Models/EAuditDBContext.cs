using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EAudit.Models
{
    public class EAuditDBContext : IdentityDbContext
    {
        public EAuditDBContext(DbContextOptions<EAuditDBContext> options) : base(options) { 
        }
    }
}
