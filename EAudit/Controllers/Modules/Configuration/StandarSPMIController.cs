using EAudit.DAO.Authentication;
using EAudit.DAO.LookUp;
using EAudit.Helpers;
using EAudit.Models;
using EAudit.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAudit.Controllers.Modules.Configurations
{
    public class StandarSPMIController : BaseController<StandarSPMIController>
    {
        private readonly ILookUp _lookupRepository;
        public StandarSPMIController(IAuthInterface authRepository, IConfiguration configuration, ILookUp lookupRepository) : base(authRepository, configuration)
        {
            _lookupRepository = lookupRepository;
        }

        [Route("/configuration/lookup/standar_spmi")]
        public IActionResult Index()
        {
            if (!User.Claims.Any())
            {
                return RedirectToAction("Login", "Account");
            }

            this.setupPage();
            ViewBag.PageDescription = "Manage Standar SPMI";
            return View("~/Views/LookUp/standar_spmi_index.cshtml");
        }

        [HttpPost]
        [Route("/configuration/lookup/standar_spmi/add")]
        public JsonResult StandarSPMI_ModalAsync_Add([FromBody] LookUp_Standar_SPMI lookUp_Standar_SPMI)
        {
            GlobalVm gvm = new GlobalVm();
            return Json(new { isValid = true, globalData = gvm });
        }

        [HttpPost]
        [Route("/configuration/lookup/standar_spmi/edit")]
        public async Task<JsonResult> StandarSPMI_ModalAsync_Edit([FromBody] LookUp_Standar_SPMI lookUp_Standar_SPMI)
        {
            GlobalVm gvm = new GlobalVm();
            gvm.spmi = await _lookupRepository.getStandarSPMI_Row(lookUp_Standar_SPMI);
            return Json(new { isValid = true, globalData = gvm });
        }


    }
}
