using Routing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Routing.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Cliente c = new Cliente("Rossi", 22);
            //ViewData["unCliente"] = c;
            ViewBag.unCliente=c;
            ViewBag.oggi = DateTime.Now.ToLongDateString();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Messaggio = "messaggio About";
            return View();
        }

    }
}