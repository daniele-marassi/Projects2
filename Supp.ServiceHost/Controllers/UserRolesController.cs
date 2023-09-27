using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Supp.ServiceHost.Repositories;
using Supp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Supp.ServiceHost.Contracts;
using Supp.ServiceHost.Contexts;
using Supp.ServiceHost.Common;
using System.Reflection;

namespace Supp.ServiceHost.Controllers
{
    [Route("api/[controller]")]
    public class UserRolesController : Controller
    {
        private IUserRolesRepository _repo;
        private SuppDatabaseContext _context;
        private readonly IConfiguration _config;

        public UserRolesController(IUserRolesRepository repo, IConfiguration config, SuppDatabaseContext context)
        {
            _repo = repo;
            _context = context;
            _config = config;
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser)]
        [HttpGet("GetAllUserRoles")] //<host>/api/UserRoles/GetAllUserRoles
        public async Task<IActionResult> GetAllUserRoles()
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            var result = await _repo.GetAllUserRoles();

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser)]
        [HttpGet("GetUserRole")] //<host>/api/UserRoles/GetUserRole/5
        public async Task<IActionResult> GetUserRole(long id)
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            var result = await _repo.GetUserRolesById(id);

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser)]
        [HttpPut("UpdateUserRole")] //<host>/api/UserRoles/UpdateUserRole/5
        public async Task<IActionResult> UpdateUserRole(long id, UserRoleDto data)
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

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

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser)]
        [HttpPost("AddUserRole")] //<host>/api/UserRoles/AddUserRole
        public async Task<IActionResult> AddUserRole(UserRoleDto data)
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddUserRole(data);

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser)]
        [HttpDelete("DeleteUserRole")] //<host>/api/UserRoles/DeleteUserRole/5
        public async Task<IActionResult> DeleteUserRole(long id)
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            var result = await _repo.DeleteUserRoleById(id);

            return Ok(result);
        }
    }
}