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
    [Route(Defaults.NodesRouteName)]
    public class NodeService : BaseService<Node, NodeEngine>
    {

        public NodeService(IServiceProvider provider)
            : base(provider)
        {
            var context = provider.GetService(typeof(AutomationContext)) as AutomationContext;

            this.Engine = new NodeEngine(context);
        }

        //GET api/nodes
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpGet]
        public List<Node> Get()
        {
            return this.Engine.Read();
        }

        // GET api/nodes/5
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpGet("{id}")]
        public Node Get(int id)
        {
            return this.Engine.Read(id);
        }

        //POST api/nodes
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpPost]
        public Node Post(Node entity)
        {
            return this.Engine.Create(entity);
        }

        // PUT api/nodes
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpPut("{id}")]
        public Node Put(long id, Node entity)
        {
            return this.Engine.Update(id, entity);
        }

        // DELETE api/nodes/5
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return this.Engine.Delete(id);
        }
    }
}
