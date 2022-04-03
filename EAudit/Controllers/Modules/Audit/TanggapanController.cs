using EAudit.DAO;
using EAudit.DAO.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace EAudit.Controllers.Modules.Audit
{
    public class TanggapanController : BaseController<TanggapanController>
    {

        public TanggapanController(IAuthInterface authRepository,
                IConfiguration configuration) :
                base(authRepository, configuration)
        {

        }
        [Route("/audit/tanggapan")]
        public IActionResult index()
        {
            if (!User.Claims.Any())
            {
                return RedirectToAction("Login", "Account");
            }

            this.setupPage();
            ViewBag.PageDescription = "Manage Tanggapan Audit";
            return View("~/Views/Audit/Audit_Tanggapan.cshtml");
        }
    }
}
