using Mair.DigitalSuite.Dashboard.Models;
using Mair.DigitalSuite.Dashboard.Common;
using Mair.DigitalSuite.Dashboard.Repositories;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using static Mair.DigitalSuite.Dashboard.Common.Config;
using Mair.DigitalSuite.Dashboard.Models.Dto;
using Mair.DigitalSuite.Dashboard.Models.Dto.Auth;

namespace Mair.DigitalSuite.Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly AuthenticationsRepository authenticationRepo;
        private readonly UsersRepository userRepo;
        private const string message = "Welcome to the Digital Suite application.";
        public static string[,] Languages;
        public static string Culture = String.Empty;
        private static Utility utility;

        public HomeController()
        {
            utility = new Utility();
            authenticationRepo = new AuthenticationsRepository();
            userRepo = new UsersRepository();
            Languages = new string[2, 2]
            {
                { "it-IT", "{0:##,###.00}" },
                { "en-US", "{0:##.###,00}" }
            };
        }

        public string[] GetLanguage(string culture)
        {
            var result = new string[2];
            for (int i = 0; i < 2; i++)
            {
                if (Languages[i, 0] == culture)
                    result = new string[2] { Languages[i, 0], Languages[i, 1] };
            }

            return result;
        }

        private void SetLanguage(string culture)
        {
            ViewBag.Language = GetLanguage(culture)[0].ToString();
            Culture = GetLanguage(culture)[0].ToString();

            HttpContext.Session.SetString("culture", Culture);

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Culture);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Culture);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Culture, false);
        }

        public ActionResult LanguageITA()
        {
            SetLanguage("it-IT");
            return View("Index");
        }

        public ActionResult LanguageENG()
        {
            SetLanguage("en-US");
            return View("Index");
        }

        public ActionResult Index()
        {
            ViewBag.Message = message;
            SetLanguage("it-IT");

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    var result = authenticationRepo.Login(dto.UserName, dto.Password).Result;

                    bool IsValidUser = result.IsAuthenticated;

                    if (IsValidUser)
                    {
                        var data = result.Data.FirstOrDefault();

                        var principal = CreatePrincipal(data);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        utility.RemoveCookie(Response, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);
                        utility.RemoveCookie(Response, GeneralSettings.Constants.MairDigitalSuiteAuthenticatedUserCookieName);

                        utility.SetCookie(Response, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName, data.Token, data.ExpiresInSeconds);
                        utility.SetCookie(Response, GeneralSettings.Constants.MairDigitalSuiteAuthenticatedUserCookieName, dto.UserName, data.ExpiresInSeconds);

                        if (data.Message != null && data.Message != String.Empty)
                        {
                            ModelState.AddModelError("ModelStateErrors", data.Message);
                            utility.AddErrorToCookie(Request, Response, data.Message);

                            return RedirectToAction("Index", "Home");
                        }
                        else
                            return RedirectToAction("DashBoard", "PlcData");
                    }
                    else
                    {
                        throw new Exception(result.Message + " - " + result.OriginalException.Message);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    return View(dto);
                    //throw;
                }

            }
        }

        private ClaimsPrincipal CreatePrincipal(TokenDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var principal = new ClaimsPrincipal();
                try
                {
                    var claims = new List<Claim>
                {
                    new Claim("UserName", dto.UserName),
                    new Claim("UserId", dto.UserId.ToString()),
                    new Claim("Roles", string.Join( ",", dto.Roles.ToArray() ) )
                };
                    principal.AddIdentity(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                    User.AddIdentity(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return principal;
            }
        }

        public ActionResult Logout()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    utility.RemoveCookie(Response, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);
                    utility.RemoveCookie(Response, GeneralSettings.Constants.MairDigitalSuiteAuthenticatedUserCookieName);
                    utility.RemoveCookie(Response, GeneralSettings.Constants.MairDigitalSuiteErrorsCookieName);

                    HttpContext.SignOutAsync();
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    utility.AddErrorToCookie(Request, Response, ex.Message);

                    return RedirectToAction("Index", "Home");
                }
            }
        }
    }
}