using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Supp.ServiceHost.Repositories;
using Supp.Models;
using Supp.ServiceHost.Services.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Supp.ServiceHost.Contracts;
using Supp.ServiceHost.Contexts;

namespace Supp.ServiceHost.Controllers
{
    [Route("api/[controller]")]
    public class GoogleAuthsController : Controller
    {
        private IGoogleAuthsRepository _repo;
        private SuppDatabaseContext _context;
        private readonly IConfiguration _config;

        public GoogleAuthsController(IGoogleAuthsRepository repo, IConfiguration config, SuppDatabaseContext context)
        {
            _repo = repo;
            _context = context;
            _config = config;
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpGet("GetAllGoogleAuths")] //<host>/api/GoogleAuths/GetAllGoogleAuths
        public async Task<IActionResult> GetAllGoogleAuths()
        {
            var result = await _repo.GetAllGoogleAuths();

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpGet("GetGoogleAuth")] //<host>/api/GoogleAuths/GetGoogleAuth/5
        public async Task<IActionResult> GetGoogleAuth(long id)
        {
            var result = await _repo.GetGoogleAuthsById(id);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpPut("UpdateGoogleAuth")] //<host>/api/GoogleAuths/UpdateGoogleAuth/5
        public async Task<IActionResult> UpdateGoogleAuth(long id, GoogleAuthDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != data.Id)
            {
                return BadRequest();
            }

            var result = await _repo.UpdateGoogleAuth(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpPost("AddGoogleAuth")] //<host>/api/GoogleAuths/AddGoogleAuth
        public async Task<IActionResult> AddGoogleAuth(GoogleAuthDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddGoogleAuth(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpDelete("DeleteGoogleAuth")] //<host>/api/GoogleAuths/DeleteGoogleAuth/5
        public async Task<IActionResult> DeleteGoogleAuth(long id)
        {
            var result = await _repo.DeleteGoogleAuthById(id);

            return Ok(result);
        }
    }
}