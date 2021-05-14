using Mair.DS.Common;
using Mair.DS.Data.Context;
using Mair.DS.Engines;
using Mair.DS.Models.Entities.Automation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mair.DS.Services.Automation.Tags
{
    [Route(Defaults.TagRouteName)]
    public class TagService : BaseService<Tag, TagEngine>
    {
        public AutomationContext context;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public TagService(IServiceProvider provider)
            : base(provider)
        {
            this.context = provider.GetService(typeof(AutomationContext)) as AutomationContext;
            this.Engine = new TagEngine(context);
        }

        //GET api/tags
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpGet]
        public List<Tag> Get()
        {
            return this.Engine.Read();
        }

        // GET api/tags/5
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpGet("{id}")]
        public Tag Get(int id)
        {
            return this.Engine.Read(id);
        }

        //POST api/tags
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpPost]
        public Tag Post(Tag entity)
        {
            return this.Engine.Create(entity);
        }

        // PUT api/tags/1
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpPut("{id}")]
        public Tag Put(long id, [FromBody]Tag entity)
        {
            return this.Engine.Update(id, entity);
        }

        // DELETE api/tags/5
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return this.Engine.Delete(id);
        }

        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpGet("getTagsPaged")] //<host>/api/tags/GetTagsPaged
        public List<Tag> GetTagsPaged(int fromRecord, int numberOfrecords)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var pagedData = new List<Tag>() { };
                try
                {
                    //filtered tags
                    pagedData = Engine.Read(fromRecord, numberOfrecords);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                }
                return pagedData;
            }
        }

        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpGet("getTagsCount")] //<host>/api/tags/GetTagsCount
        public int GetTagsCount()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var count = 0;
                try
                {
                    // get count tags
                    count = this.Engine.Read().Count;
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                }
                return count;
            }
        }
    }
}
