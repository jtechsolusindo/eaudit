using Microsoft.AspNetCore.Mvc;

namespace EAudit.Controllers
{
    public class SchedulerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
