using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Supp.ServiceHost.Common;
using Supp.ServiceHost.Contexts;
using Supp.ServiceHost.Contracts;
using Supp.ServiceHost.Models;
using Supp.ServiceHost.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;
using Additional.NLog;

namespace Supp.ServiceHost.Controllers
{
    public class HomeController : Controller
    {
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();
        private IHomeRepository _repo;

        public HomeController()
        {
            _repo = new HomeRepository();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                ApiResult result = new ApiResult() { Data = new List<ApiDto>() { }, Successful = false };
                
                IEnumerable<ApiDto> data = result.Data.AsEnumerable();

                try
                {
                    var nameSpaceControllers = "Supp.ServiceHost.Controllers";

                    result = await _repo.GetAllApi(Request, nameSpaceControllers);

                    data = result.Data.AsEnumerable();

                    return View(data);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);;

                    return View(data);
                }

                return View(data);
            }
        }
    }
}