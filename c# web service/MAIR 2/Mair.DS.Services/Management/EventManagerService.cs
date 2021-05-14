using Mair.DS.Engines.Core.EventManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Mair.DS.Services.Management
{
    [Route(Defaults.EventActionName)]
    [ApiController]
    public class EventManagerService : ControllerBase
    {
        IEventManager eventManager;
        public EventManagerService(IEventManager eventManager)
        {
            this.eventManager = eventManager;
        }

        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpGet]
        public List<string> Get()
        {
            return eventManager.GetConfiguration();
        }
    }
}
