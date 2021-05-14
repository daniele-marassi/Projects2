using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Supp.ServiceHost.Repositories;
using Supp.ServiceHost.Models;
using Supp.ServiceHost.Services.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Supp.ServiceHost.Contracts;
using Supp.ServiceHost.Contexts;

namespace Supp.ServiceHost.Controllers
{
    [Route("api/[controller]")]
    public class GoogleDriveAuthsController : Controller
    {
        private IGoogleDriveAuthsRepository _repo;
        private SuppDatabaseContext _context;
        private readonly IConfiguration _config;

        public GoogleDriveAuthsController(IGoogleDriveAuthsRepository repo, IConfiguration config, SuppDatabaseContext context)
        {
            _repo = repo;
            _context = context;
            _config = config;
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpGet("GetAllGoogleDriveAuths")] //<host>/api/GoogleDriveAuths/GetAllGoogleDriveAuths
        public async Task<IActionResult> GetAllGoogleDriveAuths()
        {
            var result = await _repo.GetAllGoogleDriveAuths();

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpGet("GetGoogleDriveAuth")] //<host>/api/GoogleDriveAuths/GetGoogleDriveAuth/5
        public async Task<IActionResult> GetGoogleDriveAuth(long id)
        {
            var result = await _repo.GetGoogleDriveAuthsById(id);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpPut("UpdateGoogleDriveAuth")] //<host>/api/GoogleDriveAuths/UpdateGoogleDriveAuth/5
        public async Task<IActionResult> UpdateGoogleDriveAuth(long id, GoogleDriveAuthDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != data.Id)
            {
                return BadRequest();
            }

            var result = await _repo.UpdateGoogleDriveAuth(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpPost("AddGoogleDriveAuth")] //<host>/api/GoogleDriveAuths/AddGoogleDriveAuth
        public async Task<IActionResult> AddGoogleDriveAuth(GoogleDriveAuthDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddGoogleDriveAuth(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpDelete("DeleteGoogleDriveAuth")] //<host>/api/GoogleDriveAuths/DeleteGoogleDriveAuth/5
        public async Task<IActionResult> DeleteGoogleDriveAuth(long id)
        {
            var result = await _repo.DeleteGoogleDriveAuthById(id);

            return Ok(result);
        }
    }
}