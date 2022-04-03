using EAudit.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAudit.DAO.AuditeeDao
{
    public interface IAuditee
    {
        public Task<List<Auditee>> getAuditeeList(string search);
    }
}
