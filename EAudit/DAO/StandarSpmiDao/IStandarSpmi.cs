using System.Collections.Generic;
using System.Threading.Tasks;
namespace EAudit.DAO.StandarSpmiDao
{
    public interface IStandarSpmi
    {
        public Task<List<Models.StandarSpmi>> getStandarList(string search);
        public Task<Models.StandarSpmi> getStandarRow(Models.StandarSpmi filter);
    }
}
