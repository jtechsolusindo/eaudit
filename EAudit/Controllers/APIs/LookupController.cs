using EAudit.Controllers.Modules;
using EAudit.DAO;
using EAudit.DAO.Authentication;
using EAudit.DAO.LookUp;
using EAudit.Models;
using EAudit.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<IActionResult> StandarSPMIImport(IFormFile file)
        {
            var list = new List<LookUp_Standar_SPMI>();
            using(var stream=new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using(var package= new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;
                    for (int row=2;row<=rowcount;row++)
                    {
                        var no_standar = worksheet.Cells[row, 1].Value.ToString().Trim();
                       
                        var data_import = new LookUp_Standar_SPMI
                        {
                            NOSTANDAR = no_standar,
                            PERNYATAAN = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            INDIKATOR = worksheet.Cells[row, 3].Value.ToString().Trim(),
                        };

                        List<LookUp_Standar_SPMI> result = await _lookupRepository.getStandarSPMI_ListByNoStandar(no_standar);
                        if (result.Count > 0)
                        {
                            data_import.ID = result[0].ID;
                        }
                        _lookupRepository.StandarSPMI_Save(data_import);
                        list.Add(data_import);

                    }
                }
            }
            return RedirectToAction("index", "StandarSPMI");
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
