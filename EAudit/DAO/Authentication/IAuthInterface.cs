using EAudit.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAudit.DAO.Authentication
{
    public interface IAuthInterface
    {
        public Task<List<UserInfo>> GetAuth(string username, string password);
        public Task SaveLog(string keterangan, string npp);
    }
        
}
