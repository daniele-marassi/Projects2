using Supp.Models;
using Supp.Site.Common;
using Supp.Site.Repositories;
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
using static Supp.Site.Common.Config;
using Additional.NLog;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System.Web;

namespace Supp.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly AuthenticationsRepository authenticationRepo;
        private readonly UsersRepository userRepo;
        private const string message = "Welcome to Supp Site.";
        public static string[,] Languages;
        public static string Culture = String.Empty;
        private static SuppUtility suppUtility;

        public HomeController()
        {
            suppUtility = new SuppUtility();
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
                var result = Authentication(dto, nLogUtility, authenticationRepo, HttpContext, User, Response, Request);

                if(result.Error == null)
                {
                    var data = result.Data;

                    if (data.Message != null && data.Message != String.Empty)
                    {
                        ModelState.AddModelError("ModelStateErrors", data.Message);
                        suppUtility.AddErrorToCookie(Request, Response, data.Message);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("ModelStateErrors", result.Error.Message);
                    return View(dto);
                }
            }
        }

        public static (bool IsValidUser, TokenDto Data, Exception Error) Authentication(LoginDto dto, NLogUtility nLogUtility, AuthenticationsRepository authenticationRepo, HttpContext httpContext, ClaimsPrincipal user, HttpResponse response, HttpRequest request)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                (bool IsValidUser, TokenDto Data, Exception Error) result;
                result.IsValidUser = false;
                result.Data = new TokenDto() { };
                result.Error = null;

                suppUtility = new SuppUtility();

                try
                {
                    var loginResult = authenticationRepo.Login(dto.UserName, dto.Password, dto.PasswordAlreadyEncrypted).Result;

                    result.IsValidUser = loginResult.IsAuthenticated;

                    if (result.IsValidUser)
                    {
                        var data = loginResult.Data.FirstOrDefault();

                        result.Data = data;

                        var principal = CreatePrincipal(data, nLogUtility, user);

                        CookieOptions option = new CookieOptions();
                        option.Expires = DateTime.Now.AddSeconds((double)data.ExpiresInSeconds);
                        var properties = new AuthenticationProperties() { AllowRefresh = true, ExpiresUtc = option.Expires, IsPersistent = true };
                        httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);

                        logger.Info("RemoveCookies - STARTED");
                        suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteAuthenticatedUserNameCookieName);
                        suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteAuthenticatedUserIdCookieName);
                        suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteClaimsCookieName);
                        logger.Info("RemoveCookies - ENDED");

                        logger.Info("SetCookies - STARTED");
                        suppUtility.SetCookie(response, GeneralSettings.Constants.SuppSiteExpiresInSecondsCookieName, data.ExpiresInSeconds.ToString(), data.ExpiresInSeconds);
                        suppUtility.SetCookie(response, GeneralSettings.Constants.SuppSiteAccessTokenCookieName, data.Token, data.ExpiresInSeconds);
                        suppUtility.SetCookie(response, GeneralSettings.Constants.SuppSiteAuthenticatedUserNameCookieName, dto.UserName, data.ExpiresInSeconds);
                        suppUtility.SetCookie(response, GeneralSettings.Constants.SuppSiteAuthenticatedUserIdCookieName, data.UserId.ToString(), data.ExpiresInSeconds);

                        logger.Info("SetCookies - ENDED");

                        logger.Info("GetClaims - STARTED");
                        var claims = SuppUtility.GetClaims(principal);
                        logger.Info("GetClaims - ENDED");

                        logger.Info("SetCookie SuppSiteClaimsCookieName - STARTED");
                        suppUtility.SetCookie(response, Config.GeneralSettings.Constants.SuppSiteClaimsCookieName, JsonConvert.SerializeObject(claims), data.ExpiresInSeconds);
                        logger.Info("SetCookie SuppSiteClaimsCookieName - ENDED");
                    }
                    else
                    {
                        var ex = new Exception(loginResult.Message + " - " + loginResult.OriginalException?.Message);
                        logger.Error(ex.ToString());
                        result.Error = ex;
                        //throw new Exception(loginResult.Message + " - " + loginResult.OriginalException?.Message);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    result.Error = ex;
                }

                return result;
            }
        }

        private static ClaimsPrincipal CreatePrincipal(TokenDto dto, NLogUtility nLogUtility, ClaimsPrincipal user)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var principal = new ClaimsPrincipal();
                try
                {
                    var claims = new List<Claim>
                    {
                        new Claim("UserName", dto.UserName),
                        new Claim("ConfigInJson", dto.ConfigInJson != null? dto.ConfigInJson : String.Empty),
                        new Claim("Name", dto.Name),
                        new Claim("Surname", dto.Surname),
                        new Claim("UserId", dto.UserId.ToString()),
                        new Claim("Roles", string.Join( ",", dto.Roles.ToArray() ) )
                    };
                    principal.AddIdentity(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                    user.AddIdentity(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
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
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteAuthenticatedUserNameCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteAuthenticatedUserIdCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteErrorsCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteClaimsCookieName);

                    HttpContext.SignOutAsync();
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    suppUtility.AddErrorToCookie(Request, Response, ex.Message);

                    return RedirectToAction("Index", "Home");
                }
            }
        }
    }
}