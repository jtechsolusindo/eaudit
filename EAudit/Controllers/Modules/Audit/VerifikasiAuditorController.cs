using EAudit.DAO;
using EAudit.DAO.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace EAudit.Controllers.Modules.Audit
{
    public class VerifikasiAuditorController : BaseController<VerifikasiAuditorController>
    {

        public VerifikasiAuditorController(IAuthInterface authRepository,
                IConfiguration configuration) :
                base(authRepository, configuration)
        {

        }
        [Route("/audit/verifikasi")]
        public IActionResult index()
        {
            if (!User.Claims.Any())
            {
                return RedirectToAction("Login", "Account");
            }

            this.setupPage();
            ViewBag.PageDescription = "Riwayat Verifikasi Auditor";
            return View("~/Views/Audit/VerifikasiAuditor.cshtml");
        }
    }
}
