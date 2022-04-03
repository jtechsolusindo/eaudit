using EAudit.Controllers.Modules;
using EAudit.DAO;
using EAudit.DAO.Authentication;
using EAudit.DAO.LookUp;
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
    public class LookupController : ControllerBase
    {
        private readonly IAuthInterface _authRepository;
        private readonly ILookUp _lookupRepository;

        public LookupController(ILookUp lookupRepository, IAuthInterface authRepository)
        {
            _authRepository = authRepository;
            _lookupRepository = lookupRepository;
        }


        [HttpPost]
        [Route("standar_spmi")]
        public async Task<IActionResult> StandarSPMI_List([FromBody] DataTableFilter filter)
        {
            List<LookUp_Standar_SPMI> result = await _lookupRepository.getStandarSPMI_List(filter.search);
            var json = System.Text.Json.JsonSerializer.Serialize(result);
            return Ok(json);
        }

        [HttpPost]
        [Route("standar_spmi/save")]
        public IActionResult StandarSPMI_Save([FromBody] LookUp_Standar_SPMI data)
        {
            AjaxResponse response = new AjaxResponse();
            try
            {
                _lookupRepository.StandarSPMI_Save(data);
                response.result = "ok";
                response.message = "Standar SPMI Berhasil Disimpan.";
            }
            catch (Exception e)
            {
                response.result = "error";
                response.message = e.Message;
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("standar_spmi/delete")]
        public IActionResult StandarSPMI_Delete([FromBody] LookUp_Standar_SPMI data)
        {
            AjaxResponse response = new AjaxResponse();
            try
            {
                _lookupRepository.StandarSPMI_Delete(data);
                response.result = "ok";
                response.message = "Standar SPMI Berhasil Dihapus.";
            }
            catch (Exception e)
            {
                response.result = "ok";
                response.message = e.Message;
            }

            return Ok(response);
        }

    }
}
