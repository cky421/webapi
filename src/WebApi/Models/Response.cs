using Newtonsoft.Json;

namespace WebApi.Models
{
    public class Response<T>
    {
        [JsonProperty("msg")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}