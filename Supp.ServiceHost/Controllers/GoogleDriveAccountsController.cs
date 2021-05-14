using System.Threading.Tasks;
using Supp.ServiceHost.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Supp.ServiceHost.Contracts;
using Supp.ServiceHost.Contexts;

namespace Supp.ServiceHost.Controllers
{
    [Route("api/[controller]")]
    public class GoogleDriveAccountsController : Controller
    {
        private IGoogleDriveAccountsRepository _repo;
        private SuppDatabaseContext _context;
        private readonly IConfiguration _config;

        public GoogleDriveAccountsController(IGoogleDriveAccountsRepository repo, IConfiguration config, SuppDatabaseContext context)
        {
            _repo = repo;
            _context = context;
            _config = config;
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpGet("GetAllGoogleDriveAccounts")] //<host>/api/GoogleDriveAccounts/GetAllGoogleDriveAccounts
        public async Task<IActionResult> GetAllGoogleDriveAccounts()
        {
            var result = await _repo.GetAllGoogleDriveAccounts();

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpGet("GetGoogleDriveAccount")] //<host>/api/GoogleDriveAccounts/GetGoogleDriveAccount/5
        public async Task<IActionResult> GetGoogleDriveAccount(long id)
        {
            var result = await _repo.GetGoogleDriveAccountsById(id);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpPut("UpdateGoogleDriveAccount")] //<host>/api/GoogleDriveAccounts/UpdateGoogleDriveAccount/5
        public async Task<IActionResult> UpdateGoogleDriveAccount(long id, GoogleDriveAccountDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != data.Id)
            {
                return BadRequest();
            }

            var result = await _repo.UpdateGoogleDriveAccount(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpPost("AddGoogleDriveAccount")] //<host>/api/GoogleDriveAccounts/AddGoogleDriveAccount
        public async Task<IActionResult> AddGoogleDriveAccount(GoogleDriveAccountDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddGoogleDriveAccount(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpDelete("DeleteGoogleDriveAccount")] //<host>/api/GoogleDriveAccounts/DeleteGoogleDriveAccount/5
        public async Task<IActionResult> DeleteGoogleDriveAccount(long id)
        {
            var result = await _repo.DeleteGoogleDriveAccountById(id);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpPost("AddGoogleDriveCredentials")] //<host>/api/GoogleDriveAccounts/AddGoogleDriveCredentials
        public async Task<IActionResult> AddGoogleDriveCredentials(string parametersJsonString)
        {
            var result = await _repo.AddGoogleDriveCredentials(parametersJsonString);

            return Ok(result);
        }
    }
}