using EAudit.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAudit.DAO.PegawaiDao
{

    public class PegawaiRepository : IPegawai
    {

        public IOptions<AppSettings> _options;

        public PegawaiRepository(IOptions<AppSettings> options)
        {
            _options = options;
        }

        public async Task<List<Pegawai>> AuditorUnassigned(string search)
        {

            DBOutput output = new DBOutput();
            output.status = true;

            DBAccess dbAccess = new DBAccess(_options);
            List<Pegawai> list = await dbAccess.ExecuteReaderAsync<Pegawai>("SP_GET_KARYAWAN_NOT_ASSIGNED");

            return list;
        }

        public async Task<List<Pegawai>> AuditeeUnassigned(string search)
        {

            DBOutput output = new DBOutput();
            output.status = true;

            DBAccess dbAccess = new DBAccess(_options);
            List<Pegawai> list = await dbAccess.ExecuteReaderAsync<Pegawai>("SP_UNASIGNED_AUDITEE");

            return list;
        }
    }
}
