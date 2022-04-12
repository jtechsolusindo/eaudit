using EAudit.DAO;
using EAudit.DAO.Authentication;
using EAudit.DAO.Person;
using EAudit.Models;
using EAudit.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAudit.Controllers.Modules.Audit
{
    public class JadwalKegiatanController : BaseController<JadwalKegiatanController>
    {
        private readonly IPerson _personRepository;
        private readonly IEAuditInterface _auditRepository;
        public JadwalKegiatanController(IAuthInterface authRepository, 
            IConfiguration configuration, 
            IEAuditInterface auditRepository, IPerson personRepository) : base(authRepository, configuration)
        {
            _auditRepository = auditRepository;
            _personRepository = personRepository;
        }
        [Route("/audit/job_schedule")]
        public IActionResult Audit_JobSchedule()
        {
            if (!User.Claims.Any())
            {
                return RedirectToAction("Login", "Account");
            }

            this.setupPage();
            ViewBag.PageDescription = "Manage Jadwal Penugasan";
            return View("~/Views/Audit/Audit_JobSchedule.cshtml");
        }

        [HttpPost]
        [Route("/audit/job_schedule/add")]
        public async Task<JsonResult> Audit_JobSchedule_Add([FromBody] Audit_JadwalKegiatan data)
        {
            GlobalVm gvm = new GlobalVm();
            List<AuditorAuditee> auditee = await _personRepository.getPersonList("", "Auditee");

            List<AuditorAuditee> list_auditee = new List<AuditorAuditee>();
            for (int i = 0; i < auditee.Count; i++)
            {
                AuditorAuditee auditorAuditee = new AuditorAuditee();
                auditorAuditee.NAMA = auditee[i].NAMA_UNIT;
                auditorAuditee.ID = auditee[i].ID;
                list_auditee.Add(auditorAuditee);
            }
            gvm.auditeeList = list_auditee;

            List<AuditorAuditee> auditor = await _personRepository.getPersonList("", "Auditor");

            List<AuditorAuditee> list_auditor = new List<AuditorAuditee>();
            for (int i = 0; i < auditor.Count; i++)
            {
                AuditorAuditee auditorAuditee = new AuditorAuditee();
                auditorAuditee.NAMA = auditor[i].NAMA_LENGKAP_GELAR;
                auditorAuditee.NPP = auditor[i].NPP.ToString();
                auditorAuditee.ID = auditor[i].ID;
                list_auditor.Add(auditorAuditee);
            }
            gvm.auditorList = list_auditor;

            return Json(new { isValid = true, globalData = gvm});
        }

        [HttpPost]
        [Route("/audit/job_schedule/edit")]
        public async Task<JsonResult> Audit_JobSchedule_Edit([FromBody] Audit_JadwalKegiatan data)
        {
            GlobalVm gvm = new GlobalVm();
            gvm.jadwalRow = await _auditRepository.getAllJadwalKegiatanRow(data.ID.Value);

            List<AuditorAuditee> auditee = await _personRepository.getPersonList("", "Auditee");

            List<AuditorAuditee> list_auditee = new List<AuditorAuditee>();
            for (int i = 0; i < auditee.Count; i++)
            {
                AuditorAuditee auditorAuditee = new AuditorAuditee();
                auditorAuditee.NAMA = auditee[i].NAMA_UNIT;
                auditorAuditee.ID = auditee[i].ID;
                list_auditee.Add(auditorAuditee);
            }
            gvm.auditeeList = list_auditee;

            List<AuditorAuditee> auditor = await _personRepository.getPersonList("", "Auditor");

            List<AuditorAuditee> list_auditor = new List<AuditorAuditee>();
            for (int i = 0; i < auditor.Count; i++)
            {
                AuditorAuditee auditorAuditee = new AuditorAuditee();
                auditorAuditee.NAMA = auditor[i].NAMA_LENGKAP_GELAR;
                auditorAuditee.NPP = auditor[i].NPP.ToString();
                auditorAuditee.ID = auditor[i].ID;
                list_auditor.Add(auditorAuditee);
            }
            gvm.auditorList = list_auditor;

            return Json(new { isValid = true, globalData = gvm, mode = "edit" });
        }
    }
}
