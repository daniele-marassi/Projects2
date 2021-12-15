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

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpGet("GetAllExecutionQueues")] //<host>/api/ExecutionQueues/GetAllExecutionQueues
        public async Task<IActionResult> GetAllExecutionQueues()
        {
            var result = await _repo.GetAllExecutionQueues();

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpGet("GetExecutionQueue")] //<host>/api/ExecutionQueues/GetExecutionQueue/5
        public async Task<IActionResult> GetExecutionQueue(long id)
        {
            var result = await _repo.GetExecutionQueuesById(id);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpPut("UpdateExecutionQueue")] //<host>/api/ExecutionQueues/UpdateExecutionQueue/5
        public async Task<IActionResult> UpdateExecutionQueue(long id, ExecutionQueueDto data)
        {
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

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpPost("AddExecutionQueue")] //<host>/api/ExecutionQueues/AddExecutionQueue
        public async Task<IActionResult> AddExecutionQueue(ExecutionQueueDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddExecutionQueue(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpDelete("DeleteExecutionQueue")] //<host>/api/ExecutionQueues/DeleteExecutionQueue/5
        public async Task<IActionResult> DeleteExecutionQueue(long id)
        {
            var result = await _repo.DeleteExecutionQueueById(id);

            return Ok(result);
        }
    }
}