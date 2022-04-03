using EAudit.DAO;
using EAudit.DAO.Authentication;
using EAudit.DAO.LookUp;
using EAudit.DAO.Person;
using EAudit.Helpers;
using EAudit.Models;
using EAudit.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EAudit.Controllers.Modules.Audit
{
    public class TemuanController : BaseController<TemuanController>
    {
        private readonly IPerson _personRepository;
        private readonly IEAuditInterface _auditRepository;
        private readonly ILookUp _lookupRepository;
        public TemuanController(IAuthInterface authRepository,
            IConfiguration configuration,
            IEAuditInterface auditRepository, IPerson personRepository, ILookUp lookupRepository) : 
            base(authRepository, configuration)
        {
            _auditRepository = auditRepository;
            _personRepository = personRepository;
            _lookupRepository = lookupRepository;

        }

        [Route("/audit/temuan")]
        public IActionResult temuan()
        {
            if (!User.Claims.Any())
            {
                return RedirectToAction("Login", "Account");
            }

            this.setupPage();
            ViewBag.PageDescription = "Manage Temuan Audit";
            return View("~/Views/Audit/Audit_Temuan.cshtml");
        }

        [HttpPost]
        [Route("/audit/temuan/listdata")]
        public async Task<IActionResult> AuditorList([FromBody] DataTableFilter filter)
        {
            _userLoggedIn = CommonHelper.userLoggedIn((ClaimsIdentity)User.Identity);
            string id_auditor = _userLoggedIn.id_auditor;
            List<AuditTemuan> result = await _auditRepository.getAllTemuan(filter.search,"Auditor", id_auditor);
            var json = System.Text.Json.JsonSerializer.Serialize(result);
            return Ok(json);
        }

        [HttpPost]
        [Route("/audit/temuan/add")]
        public async Task<JsonResult> temuanAdd()
        {
            GlobalVm gvm = new GlobalVm();
            List<AuditorAuditee> auditee = await _personRepository.getPersonList("", "Auditee");

            List<AuditorAuditee> list_auditee = new List<AuditorAuditee>();
            for (int i = 0; i < auditee.Count; i++)
            {
                AuditorAuditee auditorAuditee = new AuditorAuditee();
                auditorAuditee.NAMA = auditee[i].NAMA;
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


            List<LookUp_Standar_SPMI> spmi = await _lookupRepository.getStandarSPMI_List("");

            List<LookUp_Standar_SPMI> list_spmi = new List<LookUp_Standar_SPMI>();
            for (int i = 0; i < spmi.Count; i++)
            {
                LookUp_Standar_SPMI spmiTemp = new LookUp_Standar_SPMI();
                spmiTemp.NOSTANDAR = spmi[i].NOSTANDAR;
                spmiTemp.ID = spmi[i].ID;
                list_spmi.Add(spmiTemp);
            }
            gvm.spmiList = list_spmi;

            return Json(new { isValid = true, globalData = gvm });
        }


        [HttpPost]
        [Route("/audit/temuan/save")]
        public IActionResult Save()
        {
            var req = Request.Form;
            AjaxResponse response = new AjaxResponse();
            try
            {
                _auditRepository.temuanSave(
                    req["id_auditor"],
                    req["id_auditee"],
                    req["jenis"],
                    req["uraian"],
                    req["no_standar"]
                );
                response.result = "ok";
                response.message = "Jadwal Penugasan Berhasil Dihapus.";
            }
            catch (Exception e)
            {
                response.result = "error";
                response.message = e.Message;
            }

            return Ok(response);
        }

    }
}
