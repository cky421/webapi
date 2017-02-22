using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.Common;
using WebApi.Models.Mongodb;
using WebApi.Models.Requests;
using WebApi.Models.Responses;
using WebApi.Repositories.Interfaces;
using static WebApi.Common.Auth.ClaimsIdentityHelper;

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

        [HttpPost]
        public string Create([FromBody]CreateGroupRequest group)
        {
            var userId = this.GetUserId();
            var groupResult = _groups.InsertGroup(group.GroupName, userId);
            return JsonConvert.SerializeObject(new Response<CreateGroupResponse>
            {
                Message = groupResult.Reason,
                Data = new CreateGroupResponse
                {
                    GroupId = groupResult.Group?.GroupId,
                    GroupName = groupResult.Group?.GroupName,
                    UserId = groupResult.Group?.UserId,
                    Succeed = groupResult.Result == Result.Succeed
                }
            });

        }

        [HttpPut]
        public string Update([FromBody]UpdateGroupRequest group)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public string Delete([FromBody] DeleteGroupRequest group)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public string Fetch()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public string Fetch([FromRoute] string id)
        {
            throw new NotImplementedException();
        }
    }
}