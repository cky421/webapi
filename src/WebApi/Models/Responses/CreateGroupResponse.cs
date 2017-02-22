using Newtonsoft.Json;

namespace WebApi.Models.Responses
{
    public class CreateGroupResponse
    {
        [JsonProperty("groupname")]
        public string GroupName { get; set; }
        [JsonProperty("groupid")]
        public string GroupId { get; set; }
        [JsonProperty("userid")]
        public string UserId { get; set; }
        [JsonProperty("succeed")]
        public bool Succeed { get; set; }
    }
}