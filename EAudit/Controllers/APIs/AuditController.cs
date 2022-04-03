using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EAudit.Controllers.APIs
{
    [Route("api/configuration/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        [HttpGet]
        [Route("auditor")]
        public IEnumerable<string> GetAuditor() { 
            return new List<string>() { 
            "TEST",
            "ASD"
            };
        }
    }
}
