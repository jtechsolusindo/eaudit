using EAudit.DAO.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace EAudit.Controllers.Modules.Audit
{
    public class LogController : BaseController<LogController>
    {

        public LogController(IAuthInterface authRepository,
                IConfiguration configuration) :
                base(authRepository, configuration)
        {

        }
        [Route("/configuration/log")]
        public IActionResult index()
        {
            if (!User.Claims.Any())
            {
                return RedirectToAction("Login", "Account");
            }

            this.setupPage();
            ViewBag.PageDescription = "Log History Admin";
            return View("~/Views/Log.cshtml");
        }
    }
}
