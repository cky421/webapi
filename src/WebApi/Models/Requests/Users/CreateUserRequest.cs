using Newtonsoft.Json;

namespace WebApi.Models.Requests.Users
{
    public class CreateUserRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}