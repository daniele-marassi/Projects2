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

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpGet("GetAllSongs")] //<host>/api/Songs/GetAllSongs
        public async Task<IActionResult> GetAllSongs()
        {
            var result = await _repo.GetAllSongs();

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpGet("GetSong")] //<host>/api/Songs/GetSong/5
        public async Task<IActionResult> GetSong(long id)
        {
            var result = await _repo.GetSongsById(id);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpPut("UpdateSong")] //<host>/api/Songs/UpdateSong/5
        public async Task<IActionResult> UpdateSong(long id, SongDto data)
        {
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

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpPost("AddSong")] //<host>/api/Songs/AddSong
        public async Task<IActionResult> AddSong(SongDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddSong(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpDelete("DeleteSong")] //<host>/api/Songs/DeleteSong/5
        public async Task<IActionResult> DeleteSong(long id)
        {
            var result = await _repo.DeleteSongById(id);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpDelete("ClearSongs")] //<host>/api/Songs/ClearSongs
        public async Task<IActionResult> ClearSongs()
        {
            var result = await _repo.ClearSongs();

            return Ok(result);
        }
    }
}