using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.Common;
using WebApi.Models.QueryResult;
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
            var groupResult = _groups.InsertGroup(group?.GroupName, this.GetUserId());
            return HandleGroupResult(groupResult);
        }

        [HttpPut]
        public IActionResult Update([FromBody]UpdateGroupRequest group)
        {
            var groupResult = _groups.UpdateGroup(group?.NewGroupName, group?.GroupId, this.GetUserId());
            return HandleGroupResult(groupResult);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            var groupResult = _groups.DeleteGroup(id, this.GetUserId());
            return HandleGroupResult(groupResult);
        }

        [HttpGet]
        public IActionResult Fetch()
        {
            var groups = _groups.GetAllGroupsByUserId(this.GetUserId());
            var response = new FetchGroupResponse(groups);
            return Ok(JsonConvert.SerializeObject(response));
        }

        [HttpGet("{id}")]
        public IActionResult Fetch([FromRoute] string id)
        {
            var groupResult = _groups.GetGroup(id, this.GetUserId());
            return HandleGroupResult(groupResult);
        }

        private IActionResult HandleGroupResult(GroupResult groupResult)
        {
            var groupResponse = new GroupResponse(groupResult) {Message = groupResult.Reason};
            var content = JsonConvert.SerializeObject(groupResponse);
            IActionResult result;
            switch (groupResult.Result)
            {
                case Results.Succeed:
                    result = Ok(content);
                    break;
                case Results.NotExists:
                    result = NotFound(content);
                    break;
                case Results.Exists:
                case Results.Failed:
                    result = BadRequest(content);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return result;
        }
    }
}