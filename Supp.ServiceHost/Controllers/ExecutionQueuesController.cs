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
    public class ExecutionQueuesController : Controller
    {
        private IExecutionQueuesRepository _repo;
        private SuppDatabaseContext _context;
        private readonly IConfiguration _config;

        public ExecutionQueuesController(IExecutionQueuesRepository repo, IConfiguration config, SuppDatabaseContext context)
        {
            _repo = repo;
            _context = context;
            _config = config;
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser)]
        [HttpGet("GetAllExecutionQueues")] //<host>/api/ExecutionQueues/GetAllExecutionQueues
        public async Task<IActionResult> GetAllExecutionQueues()
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            var result = await _repo.GetAllExecutionQueues();

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser)]
        [HttpGet("GetExecutionQueue")] //<host>/api/ExecutionQueues/GetExecutionQueue/5
        public async Task<IActionResult> GetExecutionQueue(long id)
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            var result = await _repo.GetExecutionQueuesById(id);

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser)]
        [HttpPut("UpdateExecutionQueue")] //<host>/api/ExecutionQueues/UpdateExecutionQueue/5
        public async Task<IActionResult> UpdateExecutionQueue(long id, ExecutionQueueDto data)
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

            var result = await _repo.UpdateExecutionQueue(data);

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser)]
        [HttpPost("AddExecutionQueue")] //<host>/api/ExecutionQueues/AddExecutionQueue
        public async Task<IActionResult> AddExecutionQueue(ExecutionQueueDto data)
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddExecutionQueue(data);

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser)]
        [HttpDelete("DeleteExecutionQueue")] //<host>/api/ExecutionQueues/DeleteExecutionQueue/5
        public async Task<IActionResult> DeleteExecutionQueue(long id)
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            var result = await _repo.DeleteExecutionQueueById(id);

            return Ok(result);
        }
    }
}