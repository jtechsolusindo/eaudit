using EAudit.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAudit.DAO.PegawaiDao
{
    public interface IPegawai
    {
        public Task<List<Pegawai>> getPegawaiUnassigned(string search);
    }
}
