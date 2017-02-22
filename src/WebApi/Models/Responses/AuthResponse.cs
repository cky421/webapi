using Newtonsoft.Json;

namespace WebApi.Models.Responses
{
    public class AuthResponse : Response
    {
        [JsonProperty("expire")]
        public double Expire { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
    }
}