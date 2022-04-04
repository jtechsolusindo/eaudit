using EAudit.DAO;
using EAudit.DAO.Authentication;
using EAudit.Helpers;
using EAudit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace EAudit.Controllers
{
    public abstract class BaseController<T> : Controller where T : BaseController<T>
    {
        public dynamic dynamicData;

        public readonly IAuthInterface _authRepository;
        public readonly IConfiguration _configuration;
        public String _applicationName;
        public UserLoggedIn _userLoggedIn;

        /// <summary>
        /// No Desc
        /// </summary>
        private ILogger<T> _logger;
        /// <summary>
        /// No Desc
        /// </summary>
        protected ILogger<T> Logger => _logger ?? (_logger = HttpContext.RequestServices.GetService<ILogger<T>>());
        /// <summary>
        /// Nama Module
        /// </summary>
        public string baseModuleName;
        /// <summary>
        /// Nama Controller
        /// </summary>
        public string baseCtrlName;
        /// <summary>
        /// IP Address
        /// </summary>
        public string clientIPAddress;

        public void setupPage() {
            _userLoggedIn = CommonHelper.userLoggedIn((ClaimsIdentity)User.Identity);
            ViewBag.ApplicationName = _applicationName;
            ViewBag.UserName = _userLoggedIn.name;
            ViewBag.UserEmail = _userLoggedIn.email;
            ViewBag.UserNPP = _userLoggedIn.npp;
            ViewBag.IdAuditor = _userLoggedIn.id_auditor;
            ViewBag.IdAuditee = _userLoggedIn.id_auditee;
            ViewBag.Role = _userLoggedIn.role;
            ViewBag.Prodi = _userLoggedIn.prodi;
            ViewBag.Title = "Dashboard " + _userLoggedIn.role;
            MenuBuilder menuBuilder = new MenuBuilder();
            ViewBag.Menus = menuBuilder.renderMenuList(_userLoggedIn.role);
        }

        public BaseController(IAuthInterface authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
            _applicationName = configuration.GetSection("Application").GetSection("Name").Value;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}
