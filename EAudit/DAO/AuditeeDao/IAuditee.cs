using EAudit.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAudit.DAO.AuditeeDao
{
    public interface IAuditee
    {
        public Task<List<Auditee>> AuditeeList(string search, string role, string id_auditor, string id_auditee);

        public Task<Auditee> AuditeeRow(Auditee filter);

        public void AuditeeSave(Auditee data);

        public void AuditeeDelete(Auditee data);
    }
}
