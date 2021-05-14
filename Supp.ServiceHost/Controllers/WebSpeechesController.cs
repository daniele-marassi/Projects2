using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Supp.ServiceHost.Repositories;
using Supp.ServiceHost.Models;
using Supp.ServiceHost.Services.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Supp.ServiceHost.Contracts;
using Supp.ServiceHost.Contexts;

namespace Supp.ServiceHost.Controllers
{
    [Route("api/[controller]")]
    public class WebSpeechesController : Controller
    {
        private IWebSpeechesRepository _repo;
        private SuppDatabaseContext _context;
        private readonly IConfiguration _config;

        public WebSpeechesController(IWebSpeechesRepository repo, IConfiguration config, SuppDatabaseContext context)
        {
            _repo = repo;
            _context = context;
            _config = config;
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpGet("GetAllWebSpeeches")] //<host>/api/WebSpeeches/GetAllWebSpeeches
        public async Task<IActionResult> GetAllWebSpeeches()
        {
            var result = await _repo.GetAllWebSpeeches();

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpGet("GetWebSpeech")] //<host>/api/WebSpeeches/GetWebSpeech/5
        public async Task<IActionResult> GetWebSpeech(long id)
        {
            var result = await _repo.GetWebSpeechesById(id);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpPut("UpdateWebSpeech")] //<host>/api/WebSpeeches/UpdateWebSpeech/5
        public async Task<IActionResult> UpdateWebSpeech(long id, WebSpeechDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != data.Id)
            {
                return BadRequest();
            }

            var result = await _repo.UpdateWebSpeech(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpPost("AddWebSpeech")] //<host>/api/WebSpeeches/AddWebSpeech
        public async Task<IActionResult> AddWebSpeech(WebSpeechDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddWebSpeech(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpDelete("DeleteWebSpeech")] //<host>/api/WebSpeeches/DeleteWebSpeech/5
        public async Task<IActionResult> DeleteWebSpeech(long id)
        {
            var result = await _repo.DeleteWebSpeechById(id);

            return Ok(result);
        }
    }
}