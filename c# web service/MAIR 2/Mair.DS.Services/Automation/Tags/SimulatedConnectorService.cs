using Mair.DS.Common;
using Mair.DS.Data.Context;
using Mair.DS.Engines;
using Mair.DS.Models.Entities.Automation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Mair.DS.Services.Automation.Tags
{
    [Route(Defaults.SimulatedConnectorRouteName)]
    public class SimulatedConnectorService : BaseService<SimulatedConnector, SimulatedConnectorEngine>
    {
        public SimulatedConnectorService(IServiceProvider provider)
            : base (provider)
        {
            var context = provider.GetService(typeof(AutomationContext)) as AutomationContext;

            this.Engine = new SimulatedConnectorEngine(context);
        }

        // GET: api/SimulatedConnector
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpGet]
        public List<SimulatedConnector> Get()
        {
            return this.Engine.Read(); ;
        }

        // GET: api/SimulatedConnector/5
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpGet("{id}")]
        public SimulatedConnector Get(int id)
        {
            return this.Engine.Read(id);
        }

        // POST: api/SimulatedConnector
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpPost]
        public SimulatedConnector Post(SimulatedConnector entity)
        {
            return this.Engine.Create(entity);
        }

        // PUT: api/SimulatedConnector/1
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpPut("{id}")]
        public SimulatedConnector Put(long id, SimulatedConnector entity)
        {
            return this.Engine.Update(id, entity);
        }

        // DELETE: api/SimulatedConnector/5
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return this.Engine.Delete(id);
        }
    }
}
