using Mair.DS.Common;
using Mair.DS.Data.Context;
using Mair.DS.Engines;
using Mair.DS.Models.Entities.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Mair.DS.Services.Auth
{
    [Route(Defaults.RolePathsRouteName)]
    public class RolePathService : BaseService<RolePath, RolePathEngine>
    {

        public RolePathService(IServiceProvider provider)
            : base(provider)
        {
            var context = provider.GetService(typeof(AutomationContext)) as AutomationContext;

            this.Engine = new RolePathEngine(context);
        }

        //GET api/rolePaths
        [Authorize(Roles =Defaults.Roles.RoleAdmin + ", " +Defaults.Roles.RoleSuperUser + ", " +Defaults.Roles.RoleUser)]
        [HttpGet]
        public List<RolePath> Get()
        {
            return this.Engine.Read();
        }

        // GET api/rolePaths/5
        [Authorize(Roles =Defaults.Roles.RoleAdmin + ", " +Defaults.Roles.RoleSuperUser + ", " +Defaults.Roles.RoleUser)]
        [HttpGet("{id}")]
        public RolePath Get(int id)
        {
            return this.Engine.Read(id);
        }

        //POST api/rolePaths
        [Authorize(Roles =Defaults.Roles.RoleAdmin + ", " +Defaults.Roles.RoleSuperUser + ", " +Defaults.Roles.RoleUser)]
        [HttpPost]
        public RolePath Post(RolePath entity)
        {
            return this.Engine.Create(entity);
        }

        // PUT api/rolePaths
        [Authorize(Roles =Defaults.Roles.RoleAdmin + ", " +Defaults.Roles.RoleSuperUser + ", " +Defaults.Roles.RoleUser)]
        [HttpPut("{id}")]
        public RolePath Put(long id, RolePath entity)
        {
            return this.Engine.Update(id, entity);
        }

        // DELETE api/rolePaths/5
        [Authorize(Roles =Defaults.Roles.RoleAdmin + ", " +Defaults.Roles.RoleSuperUser + ", " +Defaults.Roles.RoleUser)]
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return this.Engine.Delete(id);
        }
    }
}
