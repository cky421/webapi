using System;
using Newtonsoft.Json;

namespace WebApi.Models
{
    public class Response
    {
        [JsonProperty("msg")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public object Data { get; set; }
    }
}