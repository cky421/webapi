using Newtonsoft.Json;
using WebApi.Models.QueryResult;

namespace WebApi.Models.Responses
{
    public class GroupResponse : Response
    {
        [JsonProperty("groupname")]
        public string GroupName { get; set; }
        [JsonProperty("groupid")]
        public string GroupId { get; set; }
        [JsonProperty("userid")]
        public string UserId { get; set; }

        public GroupResponse(){}

        public GroupResponse(GroupResult group)
        {
            GroupName = group.GroupName;
            GroupId = group.GroupId;
            UserId = group.UserId;
        }
    }
}