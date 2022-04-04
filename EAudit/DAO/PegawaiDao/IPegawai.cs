using EAudit.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAudit.DAO.PegawaiDao
{
    public interface IPegawai
    {
        public Task<List<Pegawai>> AuditorUnassigned(string search);

        public Task<List<Pegawai>> AuditeeUnassigned(string search);
    }
}
