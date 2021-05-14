using EURIS.Entities;
using EURISTest.Common;
using EURISTest.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace EURIS.Test.Controllers
{
    public class HomeController : Controller
    {
        private const string message = "Welcome to the EURIS Group ASP.NET MVC developer test application.";

        public static string[,] Languages;

        public static string Culture = String.Empty;

        public HomeController()
        {
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
                    result = new string[2] { Languages[i, 0],Languages[i, 1]};
            }

            return result;
        }

        public ActionResult Index()
        {
            SetLanguage("it-IT");

            return View();
        }


        private void SetLanguage(string culture)
        {
            ViewBag.Language = GetLanguage(culture)[0].ToString();
            Culture = GetLanguage(culture)[0].ToString();
            ViewBag.Message = message;
            HttpContext.Session["culture"] = Culture;
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
        
    }
}
