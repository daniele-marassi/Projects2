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
    [Route(Defaults.UserRolesRouteName)]
    public class UserRoleService : BaseService<UserRole, UserRoleEngine>
    {

        public UserRoleService(IServiceProvider provider)
            : base(provider)
        {
            var context = provider.GetService(typeof(AutomationContext)) as AutomationContext;

            this.Engine = new UserRoleEngine(context);
        }

        //GET api/userRoles
        [Authorize(Roles =Defaults.Roles.RoleAdmin + ", " +Defaults.Roles.RoleSuperUser + ", " +Defaults.Roles.RoleUser)]
        [HttpGet]
        public List<UserRole> Get()
        {
            return this.Engine.Read();
        }

        // GET api/userRoles/5
        [Authorize(Roles =Defaults.Roles.RoleAdmin + ", " +Defaults.Roles.RoleSuperUser + ", " +Defaults.Roles.RoleUser)]
        [HttpGet("{id}")]
        public UserRole Get(int id)
        {
            return this.Engine.Read(id);
        }

        //POST api/userRoles
        [Authorize(Roles =Defaults.Roles.RoleAdmin + ", " +Defaults.Roles.RoleSuperUser + ", " +Defaults.Roles.RoleUser)]
        [HttpPost]
        public UserRole Post(UserRole entity)
        {
            return this.Engine.Create(entity);
        }

        // PUT api/userRoles
        [Authorize(Roles =Defaults.Roles.RoleAdmin + ", " +Defaults.Roles.RoleSuperUser + ", " +Defaults.Roles.RoleUser)]
        [HttpPut("{id}")]
        public UserRole Put(long id, UserRole entity)
        {
            return this.Engine.Update(id, entity);
        }

        // DELETE api/userRoles/5
        [Authorize(Roles =Defaults.Roles.RoleAdmin + ", " +Defaults.Roles.RoleSuperUser + ", " +Defaults.Roles.RoleUser)]
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return this.Engine.Delete(id);
        }
    }
}
