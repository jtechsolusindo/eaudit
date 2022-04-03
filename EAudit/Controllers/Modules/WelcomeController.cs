using EAudit.Controllers;
using EAudit.DAO.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EAudit.Modules.Controllers
{
    public class WelcomeController : BaseController<WelcomeController>
    {
        public WelcomeController(IAuthInterface authRepository, IConfiguration configuration) : base(authRepository, configuration)
        {
        }

        public IActionResult Index()
        {
            return View("~/Views/Home/index_landing.cshtml");
        }
    }
}
