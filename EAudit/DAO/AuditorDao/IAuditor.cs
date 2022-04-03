using EAudit.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAudit.DAO.AuditorDao
{
    public interface IAuditor
    {
        public Task<List<Auditor>> getAuditorList(string search);

        public Task<Auditor> getAuditorRow(Auditor filter);
        public void AuditorSave(Auditor data);
        public void AuditorDelete(Auditor data);
    }
}
