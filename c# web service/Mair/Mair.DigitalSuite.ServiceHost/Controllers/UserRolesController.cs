using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Mair.DigitalSuite.ServiceHost.Repositories;
using Mair.DigitalSuite.ServiceHost.Models;
using Mair.DigitalSuite.ServiceHost.Services.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Mair.DigitalSuite.ServiceHost.Contracts;
using Mair.DigitalSuite.ServiceHost.Contexts;
using Mair.DigitalSuite.ServiceHost.Models.Dto.Auth;

namespace Mair.DigitalSuite.ServiceHost.Controllers
{
    [Route("api/[controller]")]
    public class UserRolesController : Controller
    {
        private IUserRolesRepository _repo;
        private MairDigitalSuiteDatabaseContext _context;
        private readonly IConfiguration _config;

        public UserRolesController(IUserRolesRepository repo, IConfiguration config, MairDigitalSuiteDatabaseContext context)
        {
            _repo = repo;
            _context = context;
            _config = config;
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpGet("GetAllUserRoles")] //<host>/api/UserRoles/GetAllUserRoles
        public async Task<IActionResult> GetAllUserRoles()
        {
            var result = await _repo.GetAllUserRoles();

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpGet("GetUserRole")] //<host>/api/UserRoles/GetUserRole/5
        public async Task<IActionResult> GetUserRole(long id)
        {
            var result = await _repo.GetUserRolesById(id);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpPut("UpdateUserRole")] //<host>/api/UserRoles/UpdateUserRole/5
        public async Task<IActionResult> UpdateUserRole(long id, UserRoleDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != data.Id)
            {
                return BadRequest();
            }

            var result = await _repo.UpdateUserRole(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpPost("AddUserRole")] //<host>/api/UserRoles/AddUserRole
        public async Task<IActionResult> AddUserRole(UserRoleDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddUserRole(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpDelete("DeleteUserRole")] //<host>/api/UserRoles/DeleteUserRole/5
        public async Task<IActionResult> DeleteUserRole(long id)
        {
            var result = await _repo.DeleteUserRoleById(id);

            return Ok(result);
        }
    }
}