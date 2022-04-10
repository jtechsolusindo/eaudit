using EAudit.DAO;
using EAudit.DAO.Authentication;
using EAudit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EAudit.Controllers.Modules.Audit
{
    public class VerifikasiAuditorController : BaseController<VerifikasiAuditorController>
    {
        private AuditInterface _auditRepository;
        public VerifikasiAuditorController(IAuthInterface authRepository,
                IConfiguration configuration, AuditInterface auditRepository) :
                base(authRepository, configuration)
        {
            _auditRepository = auditRepository;
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

        [Route("/audit/verifikasi/detail/{id}")]
        public async Task<IActionResult> detail(string id)
        {
            if (!User.Claims.Any())
            {
                return RedirectToAction("Login", "Account");
            }

            this.setupPage();
            VerifikasiAuditor data = await _auditRepository.VerifikasiAuditorRow(id);
            ViewBag.data_verifikasi = data;
            ViewBag.PageDescription = "Riwayat Verifikasi Auditor";
            return View("~/Views/Audit/VerifikasiAuditorDetail.cshtml");
        }
    }
}
