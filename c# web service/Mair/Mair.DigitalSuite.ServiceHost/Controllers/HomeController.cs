using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mair.DigitalSuite.ServiceHost.Common;
using Mair.DigitalSuite.ServiceHost.Contexts;
using Mair.DigitalSuite.ServiceHost.Contracts;
using Mair.DigitalSuite.ServiceHost.Models;
using Mair.DigitalSuite.ServiceHost.Models.Dto;
using Mair.DigitalSuite.ServiceHost.Models.Result;
using Mair.DigitalSuite.ServiceHost.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;

namespace Mair.DigitalSuite.ServiceHost.Controllers
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
                var result = new ApiResult() { Data = new List<ApiDto>() { }, Successful = false };
                
                IEnumerable<ApiDto> data = result.Data.AsEnumerable();

                try
                {
                    var nameSpaceControllers = "Mair.DigitalSuite.ServiceHost.Controllers";

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