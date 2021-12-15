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
    public class UsersController : Controller
    {
        private IUsersRepository _repo;
        private SuppDatabaseContext _context;
        private readonly IConfiguration _config;

        public UsersController(IUsersRepository repo, IConfiguration config, SuppDatabaseContext context)
        {
            _repo = repo;
            _context = context;
            _config = config;
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpGet("GetAllUsers")] //<host>/api/Users/GetAllUsers
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _repo.GetAllUsers();

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpGet("GetUser")] //<host>/api/Users/GetUser/5
        public async Task<IActionResult> GetUser(long id)
        {
            var result = await _repo.GetUsersById(id);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpPut("UpdateUser")] //<host>/api/Users/UpdateUser/5
        public async Task<IActionResult> UpdateUser(long id, UserDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != data.Id)
            {
                return BadRequest();
            }

            var result = await _repo.UpdateUser(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpPost("AddUser")] //<host>/api/Users/AddUser
        public async Task<IActionResult> AddUser(UserDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddUser(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpDelete("DeleteUser")] //<host>/api/Users/DeleteUser/5
        public async Task<IActionResult> DeleteUser(long id)
        {
            var result = await _repo.DeleteUserById(id);

            return Ok(result);
        }
    }
}