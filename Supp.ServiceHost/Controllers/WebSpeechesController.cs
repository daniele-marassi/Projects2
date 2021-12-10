using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Supp.ServiceHost.Repositories;
using SuppModels;
using Supp.ServiceHost.Services.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Supp.ServiceHost.Contracts;
using Supp.ServiceHost.Contexts;
using System.Security.Claims;

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

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpGet("GetAllWebSpeeches")] //<host>/api/WebSpeeches/GetAllWebSpeeches
        public async Task<IActionResult> GetAllWebSpeeches()
        {          
            var result = await _repo.GetAllWebSpeeches();

            var userId = long.Parse(User.Claims.Where(_ => _.Type == "userId").Select(_ => _.Value).FirstOrDefault());
            var roles = User.Claims.Where(_ => _.Type == ClaimsIdentity.DefaultRoleClaimType).Select(_ => _.Value).ToList();
            if (roles.Where(_ => _.Contains(Common.Config.Roles.Constants.RoleAdmin)).Count() == 0)
            {
                result.Data = result.Data.Where(_ => _.UserId == userId || _.UserId == 0).ToList();
            }

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpGet("GetWebSpeech")] //<host>/api/WebSpeeches/GetWebSpeech/5
        public async Task<IActionResult> GetWebSpeech(long id)
        {
            var result = await _repo.GetWebSpeechesById(id);

            var userId = long.Parse(User.Claims.Where(_ => _.Type == "userId").Select(_ => _.Value).FirstOrDefault());
            var roles = User.Claims.Where(_ => _.Type == ClaimsIdentity.DefaultRoleClaimType).Select(_ => _.Value).ToList();
            if (roles.Where(_ => _.Contains(Common.Config.Roles.Constants.RoleAdmin)).Count() == 0)
            {
                result.Data = result.Data.Where(_ => _.UserId == userId || _.UserId == 0).ToList();
            }

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
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

            var getWebSpeechesByIdResult = await _repo.GetWebSpeechesById(id);
            var userId = long.Parse(User.Claims.Where(_ => _.Type == "userId").Select(_ => _.Value).FirstOrDefault());
            var roles = User.Claims.Where(_ => _.Type == ClaimsIdentity.DefaultRoleClaimType).Select(_ => _.Value).ToList();
            if (roles.Where(_ => _.Contains(Common.Config.Roles.Constants.RoleAdmin)).Count() == 0)
            {
                getWebSpeechesByIdResult.Data = getWebSpeechesByIdResult.Data.Where(_ => _.UserId == userId || _.UserId == 0).ToList();
            }

            WebSpeechResult result = new WebSpeechResult() { Data = null, ResultState = ResultType.Failed, Successful = false, Message = "You do not have the rights to manage this record" };

            if (getWebSpeechesByIdResult.Data.Count() > 0) result = await _repo.UpdateWebSpeech(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
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

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpDelete("DeleteWebSpeech")] //<host>/api/WebSpeeches/DeleteWebSpeech/5
        public async Task<IActionResult> DeleteWebSpeech(long id)
        {
            var getWebSpeechesByIdResult = await _repo.GetWebSpeechesById(id);
            var userId = long.Parse(User.Claims.Where(_ => _.Type == "userId").Select(_ => _.Value).FirstOrDefault());
            var roles = User.Claims.Where(_ => _.Type == ClaimsIdentity.DefaultRoleClaimType).Select(_ => _.Value).ToList();
            if (roles.Where(_ => _.Contains(Common.Config.Roles.Constants.RoleAdmin)).Count() == 0)
            {
                getWebSpeechesByIdResult.Data = getWebSpeechesByIdResult.Data.Where(_ => _.UserId == userId || _.UserId == 0).ToList();
            }

            WebSpeechResult result = new WebSpeechResult() { Data = null, ResultState = ResultType.Failed, Successful = false, Message = "You do not have the rights to manage this record" };

            if (getWebSpeechesByIdResult.Data.Count() > 0) result = await _repo.DeleteWebSpeechById(id);

            return Ok(result);
        }
    }
}