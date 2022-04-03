using EAudit.DAO.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace EAudit.Controllers
{
    [Route("auditor")]
    [ApiController]
    public class AuditorController :  BaseController<AuditorController>
    {
        public AuditorController(IAuthInterface authRepository, IConfiguration configuration) : base(authRepository, configuration)
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
            ViewBag.PageDescription = "Manage Auditor";
            return View("~/Views/user/auditor.cshtml");
        }
    }
}
