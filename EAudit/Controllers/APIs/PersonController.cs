using EAudit.Controllers.Modules;
using EAudit.DAO.Authentication;
using EAudit.DAO.Person;
using EAudit.Models;
using EAudit.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAudit.Controllers.APIs
{
    [Route("api/configuration/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IAuthInterface _authRepository;
        private readonly IPerson _personRepository;

        public PersonController(IPerson personRepository, IAuthInterface authRepository)
        {
            _authRepository = authRepository;
            _personRepository = personRepository;
        }

        [HttpPost]
        [Route("auditor")]
        public async Task<IActionResult> AuditorList([FromBody] DataTableFilter filter)
        {
            List<AuditorAuditee> result = await _personRepository.getPersonList(filter.search, filter.searchType);
            var json = System.Text.Json.JsonSerializer.Serialize(result);
            return Ok(json);
        }

        [HttpPost]
        [Route("auditor/save")]
        public IActionResult AuditorSave([FromBody] AuditorAuditee data)
        {
            AjaxResponse response = new AjaxResponse();
            try
            {
                _personRepository.AuditorSave(data);
                response.result = "ok";
                response.message = "Auditor Berhasil Disimpan.";
            }
            catch (Exception e)
            {
                response.result = "error";
                response.message = e.Message;
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("auditor/delete")]
        public IActionResult AuditorDelete([FromBody] AuditorAuditee data)
        {
            AjaxResponse response = new AjaxResponse();
            try
            {
                _personRepository.AuditorDelete(data);
                response.result = "ok";
                response.message = "Auditor Berhasil Dihapus.";
            }
            catch (Exception e)
            {
                response.result = "ok";
                response.message = e.Message;
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("auditee")]
        public async Task<IActionResult> AuditeeList([FromBody] DataTableFilter filter)
        {
            List<AuditorAuditee> result = await _personRepository.getPersonList(filter.search, filter.searchType);
            var json = System.Text.Json.JsonSerializer.Serialize(result);
            return Ok(json);
        }

        [HttpPost]
        [Route("auditee/save")]
        public IActionResult AuditeeSave([FromBody] AuditorAuditee data)
        {
            AjaxResponse response = new AjaxResponse();
            try
            {
                _personRepository.AuditeeSave(data);
                response.result = "ok";
                response.message = "Auditee Berhasil Disimpan.";
            }
            catch (Exception e)
            {
                response.result = "error";
                response.message = e.Message;
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("auditee/delete")]
        public IActionResult AuditeeDelete([FromBody] AuditorAuditee data)
        {
            AjaxResponse response = new AjaxResponse();
            try
            {
                _personRepository.AuditeeDelete(data);
                response.result = "ok";
                response.message = "Auditee Berhasil Dihapus.";
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
