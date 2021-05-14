using Mair.DS.Common;
using Mair.DS.Data.Context;
using Mair.DS.Engines;
using Mair.DS.Models.Entities;
using Mair.DS.Models.Entities.Auth;
using Mair.DS.Models.Results.Auth;
using Mair.DS.Services.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Mair.DS.Services.Menu
{
    [Route(Defaults.MenuItemsRouteName)]
    public class MenuItemService : BaseService<MenuItem, MenuItemEngine>
    {

        public MenuItemService(IServiceProvider provider)
            : base(provider)
        {
            var context = provider.GetService(typeof(AutomationContext)) as AutomationContext;

            this.Engine = new MenuItemEngine(context);
        }

        //GET api/menuItems
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpGet]
        public List<MenuItem> Get()
        {
            return this.Engine.Read();
        }

        // GET api/menuItems/5
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpGet("{id}")]
        public MenuItem Get(int id)
        {
            return this.Engine.Read(id);
        }

        //POST api/menuItems
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpPost]
        public MenuItem Post(MenuItem entity)
        {
            return this.Engine.Create(entity);
        }

        // PUT api/menuItems
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpPut("{id}")]
        public MenuItem Put(long id, MenuItem entity)
        {
            return this.Engine.Update(id, entity);
        }

        // DELETE api/menuItems/5
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return this.Engine.Delete(id);
        }
    }
}
