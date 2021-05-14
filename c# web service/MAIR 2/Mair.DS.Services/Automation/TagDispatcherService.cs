using Mair.DS.Adapters.DataObjects;
using Mair.DS.Engines.TagDispatcher;
using Mair.DS.Models.Dto.Automation;
using Mair.DS.Models.Entities.Automation;
using Mair.DS.Models.Results.Auth;
using Mair.DS.Services.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Mair.DS.Services.Automation
{
    [Route(Defaults.TagDispatcherName)]
    [ApiController]
    public class TagDispatcherService : ControllerBase
    {
        ITagDispatcher tagDispatcher;
        public TagDispatcherService(ITagDispatcher tagDispatcher)
        {
            this.tagDispatcher = tagDispatcher;
        }

        // GET: api/TagDispatcher 
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpGet]
        public List<TagValue> Get()
        {
            return tagDispatcher.ReadAllTagsValue();
        }

        // GET: api/TagDispatcher/5
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return tagDispatcher.ReadTagValue(new Tag() { Id = id }).Value;
            //return a.Value;
        }

        // PUT: api/TagDispatcher
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpPut]
        public string Put(TagValueDto tagValueDto)
        {
            return tagDispatcher.WriteTagValue(tagValueDto.Tag, tagValueDto.Value);
        }

        // GET: api/TagDispatcher/UpdateTagsConfig
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpGet("UpdateTagsConfig")]
        public int UpdateTagsConfig()
        {
            return ((TagDispatcher)tagDispatcher).UpdateTagConfig();
        }

        // PUT: api/TagDispatcher/SubscribeDataChange
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpPut("SubscribeDataChange")]
        public bool SubscribeDataChange(Tag tag)
        {
            return tagDispatcher.SubscribeDataChange(tag);
        }

        // PUT: api/TagDispatcher/UnsubscribeDataChange
        [Authorize(Roles = Defaults.Roles.RoleAdmin + ", " + Defaults.Roles.RoleSuperUser + ", " + Defaults.Roles.RoleUser)]
        [HttpPut("UnsubscribeDataChange")]
        public bool UnsubscribeDataChange(Tag tag)
        {
            return tagDispatcher.UnsubscribeDataChange(tag);
        }
    }
}
