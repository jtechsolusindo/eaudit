using EAudit.Controllers;
using EAudit.DAO;
using EAudit.DAO.Authentication;
using EAudit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Linq;

namespace EAudit.Modules.Controllers
{
    public class HomeController : BaseController<HomeController>
    {
        public HomeController(IAuthInterface authRepository, IConfiguration configuration) : base(authRepository, configuration)
        {
        }

        public IActionResult Index()
        {
            if (!User.Claims.Any())
            {
                return RedirectToAction("Index", "Welcome");
                // return RedirectToAction("Login", "Account");
            }

            this.setupPage();
            //return Json(_userLoggedIn);
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
