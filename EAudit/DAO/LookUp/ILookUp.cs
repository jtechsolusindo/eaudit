using EAudit.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAudit.DAO.LookUp
{
    public interface ILookUp
    {
        public Task<List<LookUp_Standar_SPMI>> getStandarSPMI_List(string search);
        public Task<LookUp_Standar_SPMI> getStandarSPMI_Row(LookUp_Standar_SPMI search);
        public void StandarSPMI_Save(LookUp_Standar_SPMI data);
        public void StandarSPMI_Delete(LookUp_Standar_SPMI data);

    }
}
