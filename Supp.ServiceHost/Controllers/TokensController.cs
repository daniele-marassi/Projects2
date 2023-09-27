using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Supp.ServiceHost.Repositories;
using Supp.Models;
using Supp.ServiceHost.Services.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Supp.ServiceHost.Contracts;
using Supp.ServiceHost.Contexts;
using Microsoft.AspNetCore.Http;
using System.Reflection.PortableExecutable;
using static Supp.ServiceHost.Common.Config;
using Supp.ServiceHost.Common;
using System.Reflection;
using System;
using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using Additional.NLog;

namespace Supp.ServiceHost.Controllers
{
    [Route("api/[controller]")]
    public class TokensController : Controller
    {
        private ITokensRepository _repo;
        private SuppDatabaseContext _context;
        private readonly IConfiguration _config;
        private SuppUtility suppUtility;

        public TokensController(ITokensRepository repo, IConfiguration config, SuppDatabaseContext context)
        {
            _repo = repo;
            _context = context;
            _config = config;
            suppUtility = new SuppUtility();
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser + ", " + Config.Roles.Constants.RoleUser)]
        [HttpGet("GetAllTokens")] //<host>/api/Tokens/GetAllTokens
        public async Task<IActionResult> GetAllTokens()
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            var result = await _repo.GetAllTokens();

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser + ", " + Config.Roles.Constants.RoleUser)]
        [HttpGet("GetTokenById")] //<host>/api/Tokens/GetTokenById/5
        public async Task<IActionResult> GetTokenById(long id)
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            var result = await _repo.GetTokensById(id);

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser + ", " + Config.Roles.Constants.RoleUser)]
        [HttpGet("GetTokensByUserId")] //<host>/api/Tokens/GetTokensByUserId/5
        public async Task<IActionResult> GetTokensByUserId(long id)
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            var result = await _repo.GetTokensByUserId(id);

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser + ", " + Config.Roles.Constants.RoleUser)]
        [HttpPut("UpdateToken")] //<host>/api/Tokens/UpdateToken/5
        public async Task<IActionResult> UpdateToken(long id, TokenDto data)
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

            var result = await _repo.UpdateToken(data);

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser + ", " + Config.Roles.Constants.RoleUser)]
        [HttpPost("AddToken")] //<host>/api/Tokens/AddToken
        public async Task<IActionResult> AddToken(TokenDto data)
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.AddToken(data);

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser + ", " + Config.Roles.Constants.RoleUser)]
        [HttpPost("CleanAndAddToken")] //<host>/api/Tokens/CleanAndAddToken
        public async Task<IActionResult> CleanAndAddToken(TokenDto data)
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.CleanAndAddToken(data);

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser + ", " + Config.Roles.Constants.RoleUser)]
        [HttpDelete("DeleteToken")] //<host>/api/Tokens/DeleteToken/5
        public async Task<IActionResult> DeleteToken(long id)
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            var result = await _repo.DeleteTokenById(id);

            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser + ", " + Config.Roles.Constants.RoleUser)]
        [HttpDelete("DeleteAllTokensByUserId")] //<host>/api/Tokens/DeleteAllTokensByUserId/5
        public async Task<IActionResult> DeleteAllTokensByUserId(long userId)
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            var result = await _repo.DeleteAllTokensByUserId(userId);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetToken")] //<host>/api/Tokens/GetToken
        public async Task<IActionResult> GetToken(string userName, string password, bool passwordAlreadyEncrypted)
        {
            GrantCredentials grantCredentials = new GrantCredentials(_context);

            var result = await grantCredentials.GetToken(userName, password, passwordAlreadyEncrypted);
            return Ok(result);
        }

        [CustomAttribute("Roles", Config.Roles.Constants.RoleAdmin + ", " + Config.Roles.Constants.RoleSuperUser + ", " + Config.Roles.Constants.RoleUser)]
        [HttpGet("TokenIsValid")] //<host>/api/Tokens/TokenIsValid
        public async Task<IActionResult> TokenIsValid()
        {
            var checkAuthorizationsResult = SuppUtility.CheckAuthorizations(HttpContext.Request.Headers, SuppUtility.GetRoles(MethodInfo.GetCurrentMethod()));
            if (!checkAuthorizationsResult.IsAuthorized) return Unauthorized(checkAuthorizationsResult.Message);

            var result = true;
            return Ok(result);
        }
    }
}