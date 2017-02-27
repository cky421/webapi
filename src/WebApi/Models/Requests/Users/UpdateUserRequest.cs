using Newtonsoft.Json;

namespace WebApi.Models.Requests.Users
{
    public class UpdateUserRequest
    {
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}