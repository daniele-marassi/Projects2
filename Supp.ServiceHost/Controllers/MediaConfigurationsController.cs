﻿using System.Collections.Generic;
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
    public class MediaConfigurationsController : Controller
    {
        private IMediaConfigurationsRepository _repo;
        private SuppDatabaseContext _context;
        private readonly IConfiguration _config;

        public MediaConfigurationsController(IMediaConfigurationsRepository repo, IConfiguration config, SuppDatabaseContext context)
        {
            _repo = repo;
            _context = context;
            _config = config;
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser + ", " + Config.Roles.Constants.RoleUser)]
        [HttpGet("GetAllMediaConfigurations")] //<host>/api/MediaConfigurations/GetAllMediaConfigurations
        public async Task<IActionResult> GetAllMediaConfigurations()
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            var result = await _repo.GetAllMediaConfigurations();

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser + ", " + Config.Roles.Constants.RoleUser)]
        [HttpGet("GetMediaConfiguration")] //<host>/api/MediaConfigurations/GetMediaConfiguration/5
        public async Task<IActionResult> GetMediaConfiguration(long id)
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            var result = await _repo.GetMediaConfigurationsById(id);

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser + ", " + Config.Roles.Constants.RoleUser)]
        [HttpPut("UpdateMediaConfiguration")] //<host>/api/MediaConfigurations/UpdateMediaConfiguration/5
        public async Task<IActionResult> UpdateMediaConfiguration(long id, MediaConfigurationDto data)
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

            var result = await _repo.UpdateMediaConfiguration(data);

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser + ", " + Config.Roles.Constants.RoleUser)]
        [HttpPost("AddMediaConfiguration")] //<host>/api/MediaConfigurations/AddMediaConfiguration
        public async Task<IActionResult> AddMediaConfiguration(MediaConfigurationDto data)
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddMediaConfiguration(data);

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser + ", " + Config.Roles.Constants.RoleUser)]
        [HttpDelete("DeleteMediaConfiguration")] //<host>/api/MediaConfigurations/DeleteMediaConfiguration/5
        public async Task<IActionResult> DeleteMediaConfiguration(long id)
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            var result = await _repo.DeleteMediaConfigurationById(id);

            return Ok(result);
        }
    }
}