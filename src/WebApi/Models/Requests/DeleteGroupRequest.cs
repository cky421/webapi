using Newtonsoft.Json;

namespace WebApi.Models.Requests
{
    public class DeleteGroupRequest
    {
        [JsonProperty("groupid")]
        public string GroupId { get; set; }
    }
}