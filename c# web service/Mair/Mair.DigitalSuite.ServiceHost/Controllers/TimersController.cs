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
    public class TimersController : Controller
    {
        private ITimersRepository _repo;
        private MairDigitalSuiteDatabaseContext _context;
        private readonly IConfiguration _config;

        public TimersController(ITimersRepository repo, IConfiguration config, MairDigitalSuiteDatabaseContext context)
        {
            _repo = repo;
            _context = context;
            _config = config;
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpGet("GetAllTimers")] //<host>/api/Timers/GetAllTimers
        public async Task<IActionResult> GetAllTimers()
        {
            var result = await _repo.GetAllTimers();

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpGet("GetTimer")] //<host>/api/Timers/GetTimer/5
        public async Task<IActionResult> GetTimer(long id)
        {
            var result = await _repo.GetTimersById(id);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpPut("UpdateTimer")] //<host>/api/Timers/UpdateTimer/5
        public async Task<IActionResult> UpdateTimer(long id, TimerDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != data.Id)
            {
                return BadRequest();
            }

            var result = await _repo.UpdateTimer(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpPost("AddTimer")] //<host>/api/Timers/AddTimer
        public async Task<IActionResult> AddTimer(TimerDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddTimer(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpDelete("DeleteTimer")] //<host>/api/Timers/DeleteTimer/5
        public async Task<IActionResult> DeleteTimer(long id)
        {
            var result = await _repo.DeleteTimerById(id);

            return Ok(result);
        }
    }
}