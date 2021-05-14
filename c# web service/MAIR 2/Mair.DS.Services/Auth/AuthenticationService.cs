using Mair.DS.Common;
using Mair.DS.Data.Context;
using Mair.DS.Engines;
using Mair.DS.Models.Entities.Auth;
using Mair.DS.Models.Results.Auth;
using Mair.DS.Services.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Mair.DS.Services.Auth
{
    [Route(Defaults.AuthenticationsRouteName)]
    public class AuthenticationService : BaseService<Authentication, AuthenticationEngine>
    {

        public AuthenticationService(IServiceProvider provider)
            : base(provider)
        {
            var context = provider.GetService(typeof(AutomationContext)) as AutomationContext;

            this.Engine = new AuthenticationEngine(context);
        }

        //GET api/authentications
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpGet]
        public List<Authentication> Get()
        {
            return this.Engine.Read();
        }

        // GET api/authentications/5
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpGet("{id}")]
        public Authentication Get(int id)
        {
            return this.Engine.Read(id);
        }

        //POST api/authentications
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpPost]
        public Authentication Post(Authentication entity)
        {
            return this.Engine.Create(entity);
        }

        // PUT api/authentications
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpPut("{id}")]
        public Authentication Put(long id, Authentication entity)
        {
            return this.Engine.Update(id, entity);
        }

        // DELETE api/authentications/5
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return this.Engine.Delete(id);
        }

        [AllowAnonymous]
        [HttpGet("GetToken")] //<host>/api/authentications/GetToken
        public TokenResult GetToken(string userName, string password)
        {
            GrantCredentials grantCredentials = new GrantCredentials();

            var result = grantCredentials.GetToken(userName, password).Result;

            return result;
        }
    }
}
