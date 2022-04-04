using EAudit.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace EAudit.Helpers
{
    public class CommonHelper
    {
        public static UserLoggedIn userLoggedIn(System.Security.Claims.ClaimsIdentity claimsIdentity)
        {
            IEnumerable<System.Security.Claims.Claim> claimsN = claimsIdentity.Claims;
            List<Claim> claims = claimsN.ToList();

            UserLoggedIn userLoggedIn = new UserLoggedIn();
            userLoggedIn.name = claims[0].Value;
            userLoggedIn.npp = claims[1].Value;
            userLoggedIn.email = claims[2].Value;
            userLoggedIn.role = claims[3].Value;
            userLoggedIn.id_auditor = claims[4].Value;
            userLoggedIn.id_auditee = claims[5].Value;
            userLoggedIn.prodi = claims[6].Value;
            return userLoggedIn;
        }

    }
    
}
