using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Common;
using WebApi.Models.Requests;
using WebApi.Repositories.Interfaces;

namespace WebApi.Controllers.V1
{
    [Route("api/v1/password/group")]
    [Authorize(Config.IdentityType)]
    public class GroupController : Controller
    {
        private readonly IGroupRepository _groups;
        private readonly ILogger _logger;

        public GroupController(IGroupRepository groups, ILogger<GroupController> logger)
        {
            _groups = groups;
            _logger = logger;
        }

        [HttpPost("create")]
        public string Create([FromBody]CreateGroupRequest group)
        {
            throw new NotImplementedException();
        }

        [HttpPost("update")]
        public string Update([FromBody]UpdateGroupRequest group)
        {
            throw new NotImplementedException();
        }

        [HttpPost("delete")]
        public string Delete([FromBody] DeleteGroupRequest group)
        {
            throw new NotImplementedException();
        }

        [HttpGet("query")]
        public string Query()
        {
            throw new NotImplementedException();
        }
    }
}