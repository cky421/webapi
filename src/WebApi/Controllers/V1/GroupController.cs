using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Common;
using WebApi.Models.Requests.Groups;
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

        [HttpPost]
        public IActionResult Create([FromBody]CreateGroupRequest group)
        {
            var response = string.IsNullOrWhiteSpace(group?.GroupName)
                ? this.GenerateBadRequestResponse("groupname is null")
                : _groups.InsertGroup(this.GetUserId(), group.GroupName);

            return this.HandleResponse(response);
        }

        [HttpPut]
        public IActionResult Update([FromBody]UpdateGroupRequest group)
        {
            var response = string.IsNullOrWhiteSpace(group?.NewGroupName) ||
                       string.IsNullOrWhiteSpace(group.GroupId)
                ? this.GenerateBadRequestResponse("groupname/groupid can not be null")
                : _groups.UpdateGroup(group.GroupId, this.GetUserId(), group.NewGroupName);

            return this.HandleResponse(response);
        }

        [HttpDelete("{groupid}")]
        public IActionResult Delete([FromRoute] string groupid)
        {
            var response = string.IsNullOrWhiteSpace(groupid)
                ? this.GenerateBadRequestResponse("groupid is null")
                : _groups.DeleteGroup(groupid, this.GetUserId());

            return this.HandleResponse(response);
        }

        [HttpGet]
        public IActionResult Fetch()
        {
            var response = _groups.GetAllGroupsByUserId(this.GetUserId());
            return this.HandleResponse(response);
        }

        [HttpGet("{groupid}")]
        public IActionResult Fetch([FromRoute] string groupid)
        {
            var response = string.IsNullOrWhiteSpace(groupid)
                ? this.GenerateBadRequestResponse("groupid is null")
                : _groups.GetGroup(groupid, this.GetUserId());

            return this.HandleResponse(response);
        }
    }
}