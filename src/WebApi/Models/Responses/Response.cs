using Newtonsoft.Json;

namespace WebApi.Models.Responses
{
    public class Response
    {
        [JsonProperty("msg")]
        public string Message { get; set; }
    }
}