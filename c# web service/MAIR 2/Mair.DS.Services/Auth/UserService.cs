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
    [Route(Defaults.UsersRouteName)]
    public class UserService : BaseService<User, UserEngine>
    {

        public UserService(IServiceProvider provider)
            : base(provider)
        {
            var context = provider.GetService(typeof(AutomationContext)) as AutomationContext;

            this.Engine = new UserEngine(context);
        }

        //GET api/users
        [Authorize(Roles =Defaults.Roles.RoleAdmin + ", " +Defaults.Roles.RoleSuperUser + ", " +Defaults.Roles.RoleUser)]
        [HttpGet]
        public List<User> Get()
        {
            return this.Engine.Read();
        }

        // GET api/users/5
        [Authorize(Roles =Defaults.Roles.RoleAdmin + ", " +Defaults.Roles.RoleSuperUser + ", " +Defaults.Roles.RoleUser)]
        [HttpGet("{id}")]
        public User Get(int id)
        {
            return this.Engine.Read(id);
        }

        //POST api/users
        [Authorize(Roles =Defaults.Roles.RoleAdmin + ", " +Defaults.Roles.RoleSuperUser + ", " +Defaults.Roles.RoleUser)]
        [HttpPost]
        public User Post(User entity)
        {
            return this.Engine.Create(entity);
        }

        // PUT api/users
        [Authorize(Roles =Defaults.Roles.RoleAdmin + ", " +Defaults.Roles.RoleSuperUser + ", " +Defaults.Roles.RoleUser)]
        [HttpPut("{id}")]
        public User Put(long id, User entity)
        {
            return this.Engine.Update(id, entity);
        }

        // DELETE api/users/5
        [Authorize(Roles =Defaults.Roles.RoleAdmin + ", " +Defaults.Roles.RoleSuperUser + ", " +Defaults.Roles.RoleUser)]
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return this.Engine.Delete(id);
        }
    }
}
