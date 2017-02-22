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
        public IActionResult Create([FromBody]CreateGroupRequest group)
        {
            var userId = this.GetUserId();
            var groupResult = _groups.InsertGroup(group.GroupName, userId);
            return HandleGroupResult(groupResult);
        }

        [HttpPut]
        public IActionResult Update([FromBody]UpdateGroupRequest group)
        {
            var userId = this.GetUserId();
            var groupResult = _groups.UpdateGroup(group.NewGroupName, group.GroupId, userId);
            return HandleGroupResult(groupResult);
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] DeleteGroupRequest group)
        {
            var userId = this.GetUserId();
            var groupResult = _groups.DeleteGroup(group.GroupId, userId);
            return HandleGroupResult(groupResult);
        }

        [HttpGet]
        public IActionResult Fetch()
        {
            var userId = this.GetUserId();
            var groups = _groups.GetAllGroupsByUserId(userId);
            var response = new FetchGroupResponse(groups);
            return Ok(JsonConvert.SerializeObject(response));
        }

        [HttpGet("{id}")]
        public IActionResult Fetch([FromRoute] string id)
        {
            var userId = this.GetUserId();
            var groupResult = _groups.GetGroup(id, userId);
            return HandleGroupResult(groupResult);
        }

        private IActionResult HandleGroupResult(GroupResult groupResult)
        {
            var groupResponse = new GroupResponse(groupResult) {Message = groupResult.Reason};
            var content = JsonConvert.SerializeObject(groupResponse);
            IActionResult result = null;
            switch (groupResult.Result)
            {
                case Result.Succeed:
                    result = Ok(content);
                    break;
                case Result.Failed:
                case Result.NotExists:
                    result = NotFound(content);
                    break;
                case Result.Exists:
                    result = BadRequest(content);
                    break;
                case Result.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return result;
        }
    }
}