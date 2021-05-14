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
    public class MediaController : Controller
    {
        private IMediaRepository _repo;
        private SuppDatabaseContext _context;
        private readonly IConfiguration _config;

        public MediaController(IMediaRepository repo, IConfiguration config, SuppDatabaseContext context)
        {
            _repo = repo;
            _context = context;
            _config = config;
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpGet("GetAllMedia")] //<host>/api/Media/GetAllMedia
        public async Task<IActionResult> GetAllMedia()
        {
            var result = await _repo.GetAllMedia();

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpGet("GetMedia")] //<host>/api/Media/GetMedia/5
        public async Task<IActionResult> GetMedia(long id)
        {
            var result = await _repo.GetMediaById(id);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpPut("UpdateMedia")] //<host>/api/Media/UpdateMedia/5
        public async Task<IActionResult> UpdateMedia(long id, MediaDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != data.Id)
            {
                return BadRequest();
            }

            var result = await _repo.UpdateMedia(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpPost("AddMedia")] //<host>/api/Media/AddMedia
        public async Task<IActionResult> AddMedia(MediaDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddMedia(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpPost("AddRangeMedia")] //<host>/api/Media/AddRangeMedia
        public async Task<IActionResult> AddRangeMedia(string dataJsonString)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddRangeMedia(dataJsonString);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin + ", " + Common.Config.Roles.Constants.RoleSuperUser + ", " + Common.Config.Roles.Constants.RoleUser)]
        [HttpDelete("DeleteMedia")] //<host>/api/Media/DeleteMedia/5
        public async Task<IActionResult> DeleteMedia(long id)
        {
            var result = await _repo.DeleteMediaById(id);

            return Ok(result);
        }
    }
}