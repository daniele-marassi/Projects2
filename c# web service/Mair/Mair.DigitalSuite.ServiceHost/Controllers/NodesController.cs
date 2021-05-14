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
using Mair.DigitalSuite.ServiceHost.Models.Dto.Automation;

namespace Mair.DigitalSuite.ServiceHost.Controllers
{
    [Route("api/[controller]")]
    public class NodesController : Controller
    {
        private INodesRepository _repo;
        private MairDigitalSuiteDatabaseContext _context;
        private readonly IConfiguration _config;

        public NodesController(INodesRepository repo, IConfiguration config, MairDigitalSuiteDatabaseContext context)
        {
            _repo = repo;
            _context = context;
            _config = config;
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpGet("GetAllNodes")] //<host>/api/Nodes/GetAllNodes
        public async Task<IActionResult> GetAllNodes()
        {
            var result = await _repo.GetAllNodes();

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpGet("GetNode")] //<host>/api/Nodes/GetNode/5
        public async Task<IActionResult> GetNode(long id)
        {
            var result = await _repo.GetNodesById(id);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpPut("UpdateNode")] //<host>/api/Nodes/UpdateNode/5
        public async Task<IActionResult> UpdateNode(long id, NodeDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != data.Id)
            {
                return BadRequest();
            }

            var result = await _repo.UpdateNode(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpPost("AddNode")] //<host>/api/Nodes/AddNode
        public async Task<IActionResult> AddNode(NodeDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddNode(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpDelete("DeleteNode")] //<host>/api/Nodes/DeleteNode/5
        public async Task<IActionResult> DeleteNode(long id)
        {
            var result = await _repo.DeleteNodeById(id);

            return Ok(result);
        }
    }
}