using Newtonsoft.Json;

namespace WebApi.Models.Requests.Passwords
{
    public class CreatePasswordRequest
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("pwd")]
        public string Pwd { get; set; }
        [JsonProperty("paypwd")]
        public string PayPwd { get; set; }
        [JsonProperty("groupid")]
        public string GroupId { get; set; }
        [JsonProperty("note")]
        public string Note { get; set; }
    }
}