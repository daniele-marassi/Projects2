using System.Threading.Tasks;
using Mair.DigitalSuite.ServiceHost.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Mair.DigitalSuite.ServiceHost.Contexts;
using Mair.DigitalSuite.ServiceHost.Services.Plc;
using Mair.DigitalSuite.ServiceHost.Models.Param.Automation;

namespace Mair.DigitalSuite.ServiceHost.Controllers
{
    [Route("api/[controller]")]
    public class PlcDataController : Controller
    {
        private MairDigitalSuiteDatabaseContext _context;
        private readonly IConfiguration _config;

        public PlcDataController(IConfiguration config, MairDigitalSuiteDatabaseContext context)
        {
            _context = context;
            _config = config;
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpGet("GetPlcData")] //<host>/api/PlcData/GetPlcData
        public async Task<IActionResult> GetPlcData()
        {
            PlcManager plcManager = new PlcManager(_context);

            var result = await plcManager.GetPlcData();

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser)]
        [HttpPost("UpdatePlcData")] //<host>/api/PlcData/UpdatePlcData
        public async Task<IActionResult> UpdatePlcData(PlcDataParam param)
        {
            var driver = param.Driver;
            var connectionString = param.ConnectionString;
            var tagAddress = param.TagAddress;
            var tagValue = param.TagValue;

            PlcManager plcManager = new PlcManager(_context);

            var result = await plcManager.UpdatePlcData(driver, connectionString, tagAddress, tagValue);

            return Ok(result);
        }
    }
}