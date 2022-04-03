using EAudit.DAO.Authentication;
using EAudit.DAO.Person;
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
    public class PersonController : BaseController<PersonController>
    {
        private readonly IPerson _personRepository;
        public PersonController(IAuthInterface authRepository, IConfiguration configuration, IPerson personRepository) : base(authRepository, configuration)
        {
            _personRepository = personRepository;
        }


        [Route("/configuration/person/auditor")]
        public IActionResult Auditor()
        {
            if (!User.Claims.Any())
            {
                return RedirectToAction("Login", "Account");
            }

            this.setupPage();
            ViewBag.PageDescription = "Manage Auditor";

            return View();
        }

        [HttpPost]
        [Route("/configuration/person/auditor/add_modal")]
        public async Task<JsonResult> AuditorAdd_ModalAsync([FromBody] AuditorAuditee auditorAuditee)
        {
            GlobalVm gvm = new GlobalVm();
            gvm.employeeUnassigned = await _personRepository.getEmployeeUnListed();
            return Json(new { isValid = true, globalData = gvm });
        }

        [HttpPost]
        [Route("/configuration/person/auditor/edit_modal")]
        public async Task<JsonResult> AuditorEdit_ModalAsync([FromBody] AuditorAuditee auditorAuditee)
        {
            GlobalVm gvm = new GlobalVm();
            List<AuditorAuditee> auditors = await _personRepository.getPersonList("", "Auditor");
            List<EmployeeUnAssigned> employeeUnAssigneds = new List<EmployeeUnAssigned>();
            List<EmployeeUnAssigned> employeeUnAssigneds_ = await _personRepository.getEmployeeUnListed();
            for (int i = 0; i < auditors.Count; i++)
            {
                EmployeeUnAssigned employeeUnAssigned = new EmployeeUnAssigned();
                employeeUnAssigned.NAMA = auditors[i].NAMA_LENGKAP_GELAR;  
                employeeUnAssigned.NPP = auditors[i].NPP;
                employeeUnAssigneds.Add(employeeUnAssigned);
                
            }
            gvm.employeeUnassigned = employeeUnAssigneds.Concat(employeeUnAssigneds_).ToList();
            gvm.auditorRow = await _personRepository.getAuditorRow(auditorAuditee);
            return Json(new { isValid = true, globalData = gvm, mode = "edit" });
        }

        #region AUDITEE

        /// <summary>
        /// Menampilkan halaman daftar auditee
        /// </summary>
        /// <returns></returns>
        [Route("/configuration/person/auditee")]
        public IActionResult Auditee()
        {
            if (!User.Claims.Any())
            {
                return RedirectToAction("Login", "Account");
            }

            this.setupPage();
            ViewBag.PageDescription = "Manage Auditee";

            return View();
        }

        /// <summary>
        /// Menampilkan modal input Auditee
        /// </summary>
        /// <param name="auditorAuditee"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/configuration/person/auditee/add_modal")]
        public async Task<JsonResult> AuditeeAdd_ModalAsync([FromBody] AuditorAuditee auditorAuditee)
        {
            GlobalVm gvm = new GlobalVm();
            List<EmployeeUnAssigned> auditee = await _personRepository.getUnlistedAuditee();

            List<EmployeeUnAssigned> employeeUnAssigneds = new List<EmployeeUnAssigned>();
            for (int i = 0; i < auditee.Count; i++)
            {
                EmployeeUnAssigned employeeUnAssigned = new EmployeeUnAssigned();
                employeeUnAssigned.NAMA = auditee[i].NAMA;
                employeeUnAssigned.NPP = auditee[i].ID.ToString();
                employeeUnAssigneds.Add(employeeUnAssigned);
            }
            gvm.employeeUnassigned = employeeUnAssigneds;

            if (gvm.employeeUnassigned.Count > 0)
            {
                return Json(new { isValid = true, globalData = gvm });
            }
            else {
                return Json(new { isValid = false, globalData = gvm , message = "Semua sudah terdaftar menjadi Auditee."});
            }
            
        }

        /// <summary>
        /// Menampilkan modal edit Auditee
        /// </summary>
        /// <param name="auditorAuditee"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/configuration/person/auditee/edit_modal")]
        public async Task<JsonResult> AuditerrEdit_ModalAsync([FromBody] AuditorAuditee auditorAuditee)
        {
            GlobalVm gvm = new GlobalVm();

            List<AuditorAuditee> auditee = await _personRepository.getPersonList("", "Auditee");
            List<EmployeeUnAssigned> employeeUnAssigneds = new List<EmployeeUnAssigned>();
            for (int i = 0; i < auditee.Count; i++)
            {
                EmployeeUnAssigned employeeUnAssigned = new EmployeeUnAssigned();
                employeeUnAssigned.NAMA = auditee[i].NAMA_UNIT;
                employeeUnAssigned.NPP = auditee[i].ID_UNIT.Value.ToString();
                employeeUnAssigneds.Add(employeeUnAssigned);
            }
            gvm.employeeUnassigned = employeeUnAssigneds;
            gvm.auditeeRow = await _personRepository.getAuditeeRow(auditorAuditee);
            return Json(new { isValid = true, globalData = gvm, mode = "edit" });
        }


        #endregion
        //[Route("/configuration/person/auditor/edit")]
        //public async Task<IActionResult> PersonEdit(int? id)
        //{
        //    if (!User.Claims.Any())
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    this.setupPage();
        //    ViewBag.PageDescription = (id.HasValue ? "Edit " : "Tambah ") + "Auditor";
        //    ViewData["EmployeeList"] = await _personRepository.getEmployeeUnListed();
        //    ViewData["personType"] = "auditor";


        //    return View();
        //}
    }
}
