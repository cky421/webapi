using Newtonsoft.Json;

namespace WebApi.Models.Requests
{
    public class CreateGroupRequest
    {
        [JsonProperty("groupname")]
        public string GroupName { get; set; }
    }
}