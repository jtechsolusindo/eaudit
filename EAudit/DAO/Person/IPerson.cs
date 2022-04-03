using EAudit.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAudit.DAO.Person
{
    public interface IPerson
    {
        public Task<List<AuditorAuditee>> getPersonList(string search, string role = "Admin/Auditee/Auditor");
        public Task<AuditorAuditee> getAuditorRow(AuditorAuditee filter);
        public Task<AuditorAuditee> getAuditeeRow(AuditorAuditee filter);
        public Task<List<EmployeeUnAssigned>> getEmployeeUnListed();
        public Task<List<EmployeeUnAssigned>> getUnlistedAuditee();
        public void AuditorSave(AuditorAuditee data);
        public void AuditeeSave(AuditorAuditee data);
        public void AuditorDelete(AuditorAuditee data);
        public void AuditeeDelete(AuditorAuditee data);
    }
}
