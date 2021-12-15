using System.Threading.Tasks;
using Supp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Supp.ServiceHost.Contracts;
using Supp.ServiceHost.Contexts;

namespace Supp.ServiceHost.Controllers
{
    [Route("api/[controller]")]
    public class GoogleAccountsController : Controller
    {
        private IGoogleAccountsRepository _repo;
        private SuppDatabaseContext _context;
        private readonly IConfiguration _config;

        public GoogleAccountsController(IGoogleAccountsRepository repo, IConfiguration config, SuppDatabaseContext context)
        {
            _repo = repo;
            _context = context;
            _config = config;
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpGet("GetAllGoogleAccounts")] //<host>/api/GoogleAccounts/GetAllGoogleAccounts
        public async Task<IActionResult> GetAllGoogleAccounts()
        {
            var result = await _repo.GetAllGoogleAccounts();

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpGet("GetGoogleAccount")] //<host>/api/GoogleAccounts/GetGoogleAccount/5
        public async Task<IActionResult> GetGoogleAccount(long id)
        {
            var result = await _repo.GetGoogleAccountsById(id);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpPut("UpdateGoogleAccount")] //<host>/api/GoogleAccounts/UpdateGoogleAccount/5
        public async Task<IActionResult> UpdateGoogleAccount(long id, GoogleAccountDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != data.Id)
            {
                return BadRequest();
            }

            var result = await _repo.UpdateGoogleAccount(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpPost("AddGoogleAccount")] //<host>/api/GoogleAccounts/AddGoogleAccount
        public async Task<IActionResult> AddGoogleAccount(GoogleAccountDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddGoogleAccount(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpDelete("DeleteGoogleAccount")] //<host>/api/GoogleAccounts/DeleteGoogleAccount/5
        public async Task<IActionResult> DeleteGoogleAccount(long id)
        {
            var result = await _repo.DeleteGoogleAccountById(id);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpPost("AddGoogleCredentials")] //<host>/api/GoogleAccounts/AddGoogleCredentials
        public async Task<IActionResult> AddGoogleCredentials(string parametersJsonString)
        {
            var result = await _repo.AddGoogleCredentials(parametersJsonString);

            return Ok(result);
        }
    }
}