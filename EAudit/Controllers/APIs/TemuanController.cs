using EAudit.Controllers.Modules;
using EAudit.DAO;
using EAudit.Models;
using EAudit.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAudit.Controllers.APIs
{
    [Route("api/audit/[controller]")]
    [ApiController]
    public class TemuanController : ControllerBase
    {
        private readonly IEAuditInterface _auditRepository;

        public TemuanController(IEAuditInterface auditRepository)
        {
            _auditRepository = auditRepository;
        }

        

        [HttpPost]
        [Route("penugasan/save")]
        public IActionResult Save([FromBody] Audit_JadwalKegiatan data)
        {
            AjaxResponse response = new AjaxResponse();
            try
            {
                _auditRepository.JadwalKegiatanSave(data);
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
