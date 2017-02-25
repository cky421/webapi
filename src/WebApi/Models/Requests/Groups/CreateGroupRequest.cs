using Newtonsoft.Json;

namespace WebApi.Models.Requests.Groups
{
    public class CreateGroupRequest
    {
        [JsonProperty("groupname")]
        public string GroupName { get; set; }
    }
}