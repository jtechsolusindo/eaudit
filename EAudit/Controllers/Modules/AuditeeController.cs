using EAudit.DAO.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace EAudit.Controllers.Modules
{
    [Route("auditee")]
    public class AuditeeController : BaseController<AuditeeController>
    {
        public AuditeeController(IAuthInterface authRepository, IConfiguration configuration) : base(authRepository, configuration)
        {
        }

        [HttpGet]
        [Route("data")]
        public IActionResult index()
        {
            if (!User.Claims.Any())
            {
                return RedirectToAction("Login", "Account");
            }

            this.setupPage();
            ViewBag.PageDescription = "Manage Auditee";
            return View("~/Views/Auditee/Auditee.cshtml");
        }
    }
}
