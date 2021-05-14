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
    public class EventsController : Controller
    {
        private IEventsRepository _repo;
        private MairDigitalSuiteDatabaseContext _context;
        private readonly IConfiguration _config;

        public EventsController(IEventsRepository repo, IConfiguration config, MairDigitalSuiteDatabaseContext context)
        {
            _repo = repo;
            _context = context;
            _config = config;
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpGet("GetAllEvents")] //<host>/api/Events/GetAllEvents
        public async Task<IActionResult> GetAllEvents()
        {
            var result = await _repo.GetAllEvents();

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpGet("GetEvent")] //<host>/api/Events/GetEvent/5
        public async Task<IActionResult> GetEvent(long id)
        {
            var result = await _repo.GetEventsById(id);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpPut("UpdateEvent")] //<host>/api/Events/UpdateEvent/5
        public async Task<IActionResult> UpdateEvent(long id, EventDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != data.Id)
            {
                return BadRequest();
            }

            var result = await _repo.UpdateEvent(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpPost("AddEvent")] //<host>/api/Events/AddEvent
        public async Task<IActionResult> AddEvent(EventDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddEvent(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpDelete("DeleteEvent")] //<host>/api/Events/DeleteEvent/5
        public async Task<IActionResult> DeleteEvent(long id)
        {
            var result = await _repo.DeleteEventById(id);

            return Ok(result);
        }
    }
}