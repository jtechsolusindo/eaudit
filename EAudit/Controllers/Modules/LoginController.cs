using Microsoft.AspNetCore.Mvc;

namespace EAudit.Modules.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
