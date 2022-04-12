using E_AuditInternal.Services;
using EAudit.Controllers.Modules;
using EAudit.DAO;
using EAudit.DAO.AuditorDao;
using EAudit.Helpers;
using EAudit.Models;
using EAudit.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EAudit.Controllers.APIs
{
    [Route("api/audit/[controller]")]
    [ApiController]
    public class SchedulerController : ControllerBase
    {
        private readonly IEAuditInterface _auditRepository;
        private IAuditor _auditorRepository;
        private readonly IMailService mailService;
        private static bool is_debug = false;
        private static string email_debug = "lawrencenoman@gmail.com";

        public SchedulerController(IEAuditInterface auditRepository, IMailService mailService, IAuditor auditorRepository)
        {
            _auditRepository = auditRepository;
            this.mailService = mailService;
            _auditorRepository = auditorRepository;
        }

        [HttpPost]
        [Route("penugasan")]
        public async Task<IActionResult> AuditorList([FromBody] DataTableFilter filter)
        {
            UserLoggedIn _userLoggedIn = CommonHelper.userLoggedIn((ClaimsIdentity)User.Identity);
            string id_auditor = _userLoggedIn.id_auditor;
            string id_auditee = _userLoggedIn.id_auditee;
            string role = _userLoggedIn.role;
            List<Audit_JadwalKegiatan> result = await _auditRepository.getAllJadwalKegiatanList(filter.search,role,id_auditor);
            var json = System.Text.Json.JsonSerializer.Serialize(result);
            return Ok(json);
        }

        [HttpPost]
        [Route("penugasan/save")]
        public async Task<IActionResult> Save([FromBody] Audit_JadwalKegiatan data)
        {
            AjaxResponse response = new AjaxResponse();
            try
            {
                _auditRepository.JadwalKegiatanSave(data);
                var id_audiator_array = data.ID_AUDITOR.Split("#");
                foreach(var id_audiator in id_audiator_array)
                {
                    Auditor filter = new Auditor();
                    filter.ID = int.Parse(id_audiator);
                    Auditor data_audiator = await _auditorRepository.AuditorRow(filter);
                    var email = data_audiator.EMAIL;
                    MailRequest request = new MailRequest();
                    request.ToEmail = is_debug ? email_debug : email;
                    request.Subject = "Jadwal Baru Ditambahkan";
                    request.Body = String.Format(@"Halo, {0}<br/>Email ini adalah pemberitahuan bahwa ada Jadwal yang telah di-assign kepada Anda pada Sistem e-Audit:
                    <br/>
                    <table width='100%'>
                    <tr>
                    <td>Auditee</td>
                    <td valign='top'>: {1}</td>
                    </tr>
                    <tr>
                    <td>Tanggal</td>
                    <td valign='top'>: {2}</td>
                    </tr>
                    <tr>
                    <td>Jam Mulai</td>
                    <td valign='top'>: {3}</td>
                    </tr>
                    <tr>
                    <td>Jam Selesai</td>
                    <td valign='top'>: {4}</td>
                    </tr>
                    </table>", data_audiator.NAMA_LENGKAP_GELAR.ToString(), data.NAMA_UNIT.ToString(), data.TANGGAL.ToString(), data.WS, data.WE.ToString());
                    await mailService.SendEmailAsync(request);
                }
                response.result = "ok";
                response.message = "Jadwal Penugasan Berhasil Disimpan.";
            }
            catch (Exception e)
            {
                response.result = "error";
                response.message = e.Message;
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("penugasan/delete")]
        public IActionResult Delete([FromBody] Audit_JadwalKegiatan data)
        {
            AjaxResponse response = new AjaxResponse();
            try
            {
                _auditRepository.HapusJadwal(data);
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
