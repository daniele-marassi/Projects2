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
    [Route(Defaults.RolesRouteName)]
    public class RoleService : BaseService<Role, RoleEngine>
    {

        public RoleService(IServiceProvider provider)
            : base(provider)
        {
            var context = provider.GetService(typeof(AutomationContext)) as AutomationContext;

            this.Engine = new RoleEngine(context);
        }

        //GET api/roles
        [Authorize(Roles =Defaults.Roles.RoleAdmin + ", " +Defaults.Roles.RoleSuperUser + ", " +Defaults.Roles.RoleUser)]
        [HttpGet]
        public List<Role> Get()
        {
            return this.Engine.Read();
        }

        // GET api/roles/5
        [Authorize(Roles =Defaults.Roles.RoleAdmin + ", " +Defaults.Roles.RoleSuperUser + ", " +Defaults.Roles.RoleUser)]
        [HttpGet("{id}")]
        public Role Get(int id)
        {
            return this.Engine.Read(id);
        }

        //POST api/roles
        [Authorize(Roles =Defaults.Roles.RoleAdmin + ", " +Defaults.Roles.RoleSuperUser + ", " +Defaults.Roles.RoleUser)]
        [HttpPost]
        public Role Post(Role entity)
        {
            return this.Engine.Create(entity);
        }

        // PUT api/roles
        [Authorize(Roles =Defaults.Roles.RoleAdmin + ", " +Defaults.Roles.RoleSuperUser + ", " +Defaults.Roles.RoleUser)]
        [HttpPut("{id}")]
        public Role Put(long id, Role entity)
        {
            return this.Engine.Update(id, entity);
        }

        // DELETE api/roles/5
        [Authorize(Roles =Defaults.Roles.RoleAdmin + ", " +Defaults.Roles.RoleSuperUser + ", " +Defaults.Roles.RoleUser)]
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return this.Engine.Delete(id);
        }
    }
}
