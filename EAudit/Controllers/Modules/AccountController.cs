﻿using EAudit.DAO;
using EAudit.DAO.Authentication;
using EAudit.Helpers;
using EAudit.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EAudit.Modules.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthInterface _authRepository;

        public AccountController(IAuthInterface authRepository)
        {
            _authRepository = authRepository;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "Login :: Audit Internal KPM";
            return View();
        }
        public IActionResult Login()
        {
            ViewBag.Title = "Login :: Audit Internal KPM";
            return View();
        }

        [HttpPost]
        [Route("/account/doLogin")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> doLogin(string username, string password)
        {
            TempData["username"] = username;
            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TempData["err_message"] = "Gagal Login! Username dan Password Tidak Boleh Kosong.";
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<UserInfo> userLogin = await _authRepository.GetAuth(username, password);
                //return Json(userLogin);
                if (userLogin.Count == 0)
                {
                    TempData["err_message"] = "Gagal Login! Username dan Password Tidak Dikenal.";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    UserInfo userInfo = userLogin[0];
                   
                    ClaimsIdentity identity = new ClaimsIdentity();

                    identity = new ClaimsIdentity(new[] {
                                    new Claim(ClaimTypes.Name, userInfo.NAMA),
                                    new Claim("username", userInfo.NPP),
                                    new Claim("email", userInfo.EMAIL == null ? "localhost":userInfo.EMAIL),
                                    new Claim("role", userInfo.ROLE),
                                    new Claim("id_auditor", userInfo.ID_AUDITOR!=null?userInfo.ID_AUDITOR.Value.ToString():""),
                                    new Claim("id_auditee", userInfo.ID_AUDITEE!=null?userInfo.ID_AUDITEE.Value.ToString():""),
                                    new Claim("prodi", userInfo.PRODI!=null?userInfo.PRODI.ToString():""),
                                }, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                }
            }

            return RedirectToAction("index", "home");
        }

        public IActionResult LogOut()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            UserLoggedIn userLoggedIn = CommonHelper.userLoggedIn((ClaimsIdentity)User.Identity);
            _authRepository.SaveLog("Logout " + userLoggedIn.role, userLoggedIn.npp);

            return RedirectToAction("login", "account");
        }

    }
}
