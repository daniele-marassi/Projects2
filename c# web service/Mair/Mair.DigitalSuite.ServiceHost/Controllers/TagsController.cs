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
    public class TagsController : Controller
    {
        private ITagsRepository _repo;
        private MairDigitalSuiteDatabaseContext _context;
        private readonly IConfiguration _config;

        public TagsController(ITagsRepository repo, IConfiguration config, MairDigitalSuiteDatabaseContext context)
        {
            _repo = repo;
            _context = context;
            _config = config;
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpGet("GetAllTags")] //<host>/api/Tags/GetAllTags
        public async Task<IActionResult> GetAllTags()
        {
            var result = await _repo.GetAllTags();

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpGet("GetTag")] //<host>/api/Tags/GetTag/5
        public async Task<IActionResult> GetTag(long id)
        {
            var result = await _repo.GetTagsById(id);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpPut("UpdateTag")] //<host>/api/Tags/UpdateTag/5
        public async Task<IActionResult> UpdateTag(long id, TagDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != data.Id)
            {
                return BadRequest();
            }

            var result = await _repo.UpdateTag(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpPost("AddTag")] //<host>/api/Tags/AddTag
        public async Task<IActionResult> AddTag(TagDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddTag(data);

            return Ok(result);
        }

        [Authorize(Roles = Common.Config.Roles.Constants.RoleAdmin)]
        [HttpDelete("DeleteTag")] //<host>/api/Tags/DeleteTag/5
        public async Task<IActionResult> DeleteTag(long id)
        {
            var result = await _repo.DeleteTagById(id);

            return Ok(result);
        }
    }
}