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

using Microsoft.AspNetCore.Authentication.Cookies;
using static Supp.Site.Common.Config;
using Additional.NLog;

using Newtonsoft.Json;
using System.Web;
using GoogleManagerModels;
using static Google.Apis.Requests.BatchRequest;
using System.Security.Cryptography;
using Additional;

namespace Supp.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly AuthenticationsRepository authenticationRepo;
        private const string message = "Welcome to Supp Site.";
        public static string[,] Languages;
        public static string Culture = String.Empty;
        private static SuppUtility suppUtility;
        private static Utility utility;

        public HomeController()
        {
            suppUtility = new SuppUtility();
            authenticationRepo = new AuthenticationsRepository();
            utility = new Utility();

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
                var result = Authentication(dto, nLogUtility, authenticationRepo, HttpContext, Response, Request);

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

        public static (bool IsValidUser, TokenDto Data, Exception Error) Authentication(LoginDto dto, NLogUtility nLogUtility, AuthenticationsRepository authenticationRepo, HttpContext httpContext, HttpResponse response, HttpRequest request)
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
                    var tokenRepo = new TokensRepository();

                    var loginResult = tokenRepo.Login(dto.UserName, dto.Password, dto.PasswordAlreadyEncrypted).Result;

                    result.IsValidUser = loginResult.IsAuthenticated;

                    if (result.IsValidUser)
                    {
                        var data = loginResult.Data.FirstOrDefault();

                        result.Data = data;

                        SuppUtility.TokenStorage(data, nLogUtility);

                        logger.Info("RemoveCookies - STARTED");
                        suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteAuthenticatedUserIdCookieName);
                        suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteAuthenticatedUserNameCookieName);
                        suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteErrorsCookieName);
                        suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteExpiresInSecondsCookieName);
                        suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteHostSelectedCookieName);
                        suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteApplicationCookieName);
                        suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteAlwaysShowCookieName);
                        suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteTokenDtoCookieName);
                        suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteLoadDateCookieName);
                        suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteAuthenticatedPasswordCookieName);
                        suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);
                        suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteNewWebSpeechDtoInJsonCookieName);

                        logger.Info("RemoveCookies - ENDED");

                        logger.Info("SetCookies - STARTED");
                        suppUtility.SetCookie(response, GeneralSettings.Constants.SuppSiteExpiresInSecondsCookieName, data.ExpiresInSeconds.ToString(), data.ExpiresInSeconds);
                        suppUtility.SetCookie(response, GeneralSettings.Constants.SuppSiteAccessTokenCookieName, data.TokenCode, data.ExpiresInSeconds);
                        suppUtility.SetCookie(response, GeneralSettings.Constants.SuppSiteAuthenticatedUserIdCookieName, data.UserId.ToString(), data.ExpiresInSeconds);
                        suppUtility.SetCookie(response, GeneralSettings.Constants.SuppSiteAuthenticatedUserNameCookieName, dto.UserName, data.ExpiresInSeconds);
                        
                        var passwordMd5 = "";

                        if (!dto.PasswordAlreadyEncrypted)
                        {
                            using (MD5 md5Hash = MD5.Create())
                            {
                                passwordMd5 = utility.GetMd5Hash(md5Hash, dto.Password);
                            }
                        }
                        else
                            passwordMd5 = dto.Password;

                        suppUtility.SetCookie(response, GeneralSettings.Constants.SuppSiteAuthenticatedPasswordCookieName, passwordMd5, data.ExpiresInSeconds);

                        logger.Info("SetCookies - ENDED");

                        logger.Info("SetCookie SuppSiteTokenDtoCookieName - STARTED");
                        suppUtility.SetCookie(response, Config.GeneralSettings.Constants.SuppSiteTokenDtoCookieName, JsonConvert.SerializeObject(data), data.ExpiresInSeconds);
                        logger.Info("SetCookie SuppSiteTokenDtoCookieName - ENDED");
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

        public ActionResult Logout()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteAuthenticatedUserIdCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteAuthenticatedUserNameCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteErrorsCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteExpiresInSecondsCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteHostSelectedCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteApplicationCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteAlwaysShowCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteTokenDtoCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteLoadDateCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteAuthenticatedPasswordCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteNewWebSpeechDtoInJsonCookieName);


                    HttpContext.SignOutAsync();
                    return RedirectToAction("Login", "Home");
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