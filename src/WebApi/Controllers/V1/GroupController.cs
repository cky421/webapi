using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.Common;
using WebApi.Models.Requests.Groups;
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
            var groupResponse = _groups.InsertGroup(group?.GroupName, this.GetUserId());
            return HandleGroupResponse(groupResponse);
        }

        [HttpPut]
        public IActionResult Update([FromBody]UpdateGroupRequest group)
        {
            var groupResponse = _groups.UpdateGroup(group?.NewGroupName, group?.GroupId, this.GetUserId());
            return HandleGroupResponse(groupResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            var groupResponse = _groups.DeleteGroup(id, this.GetUserId());
            return HandleGroupResponse(groupResponse);
        }

        [HttpGet]
        public IActionResult Fetch()
        {
            var groups = _groups.GetAllGroupsByUserId(this.GetUserId());
            return HandleGroupResponse(groups);
        }

        [HttpGet("{id}")]
        public IActionResult Fetch([FromRoute] string id)
        {
            var groupResponse = _groups.GetGroup(id, this.GetUserId());
            return HandleGroupResponse(groupResponse);
        }

        private IActionResult HandleGroupResponse(Response groupResponse)
        {
            var content = JsonConvert.SerializeObject(groupResponse);
            IActionResult result;
            switch (groupResponse.Result)
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