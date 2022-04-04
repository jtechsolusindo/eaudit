using EAudit.Controllers;
using EAudit.DAO;
using EAudit.DAO.Authentication;
using EAudit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EAudit.Modules.Controllers
{
    public class HomeController : BaseController<HomeController>
    {
        private AuditInterface _auditRepository;
        public HomeController(IAuthInterface authRepository, IConfiguration configuration, AuditInterface auditRepository) : base(authRepository, configuration)
        {
            _auditRepository = auditRepository;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Claims.Any())
            {
                return RedirectToAction("Index", "Welcome");
                // return RedirectToAction("Login", "Account");
            }
            List<Dashboard> list_data = await _auditRepository.DashboardList();
            var json = System.Text.Json.JsonSerializer.Serialize(list_data);
            //return Ok(json);
            
            ViewBag.data_dashbord = list_data;
            this.setupPage();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
