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
    public class SongsController : Controller
    {
        private ISongsRepository _repo;
        private SuppDatabaseContext _context;
        private readonly IConfiguration _config;

        public SongsController(ISongsRepository repo, IConfiguration config, SuppDatabaseContext context)
        {
            _repo = repo;
            _context = context;
            _config = config;
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser)]
        [HttpGet("GetAllSongs")] //<host>/api/Songs/GetAllSongs
        public async Task<IActionResult> GetAllSongs()
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            var result = await _repo.GetAllSongs();

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser)]
        [HttpGet("GetSong")] //<host>/api/Songs/GetSong/5
        public async Task<IActionResult> GetSong(long id)
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            var result = await _repo.GetSongsById(id);

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser)]
        [HttpPut("UpdateSong")] //<host>/api/Songs/UpdateSong/5
        public async Task<IActionResult> UpdateSong(long id, SongDto data)
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

            var result = await _repo.UpdateSong(data);

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser)]
        [HttpPost("AddSong")] //<host>/api/Songs/AddSong
        public async Task<IActionResult> AddSong(SongDto data)
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddSong(data);

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser)]
        [HttpDelete("DeleteSong")] //<host>/api/Songs/DeleteSong/5
        public async Task<IActionResult> DeleteSong(long id)
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            var result = await _repo.DeleteSongById(id);

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser)]
        [HttpDelete("ClearSongs")] //<host>/api/Songs/ClearSongs
        public async Task<IActionResult> ClearSongs()
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            var result = await _repo.ClearSongs();

            return Ok(result);
        }
    }
}