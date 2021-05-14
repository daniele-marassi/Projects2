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
    public class UserRoleTypesController : Controller
    {
        private IUserRoleTypesRepository _repo;
        private MairDigitalSuiteDatabaseContext _context;
        private readonly IConfiguration _config;

        public UserRoleTypesController(IUserRoleTypesRepository repo, IConfiguration config, MairDigitalSuiteDatabaseContext context)
        {
            _repo = repo;
            _context = context;
            _config = config;
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpGet("GetAllUserRoleTypes")] //<host>/api/UserRoleTypes/GetAllUserRoleTypes
        public async Task<IActionResult> GetAllUserRoleTypes()
        {
            var result = await _repo.GetAllUserRoleTypes();

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpGet("GetUserRoleType")] //<host>/api/UserRoleTypes/GetUserRoleType/5
        public async Task<IActionResult> GetUserRoleType(long id)
        {
            var result = await _repo.GetUserRoleTypesById(id);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpPut("UpdateUserRoleType")] //<host>/api/UserRoleTypes/UpdateUserRoleType/5
        public async Task<IActionResult> UpdateUserRoleType(long id, UserRoleTypeDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != data.Id)
            {
                return BadRequest();
            }

            var result = await _repo.UpdateUserRoleType(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpPost("AddUserRoleType")] //<host>/api/UserRoleTypes/AddUserRoleType
        public async Task<IActionResult> AddUserRoleType(UserRoleTypeDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddUserRoleType(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpDelete("DeleteUserRoleType")] //<host>/api/UserRoleTypes/DeleteUserRoleType/5
        public async Task<IActionResult> DeleteUserRoleType(long id)
        {
            var result = await _repo.DeleteUserRoleTypeById(id);

            return Ok(result);
        }
    }
}