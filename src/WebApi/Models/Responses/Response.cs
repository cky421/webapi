using Newtonsoft.Json;

namespace WebApi.Models.Responses
{
    public class Response
    {
        [JsonProperty("msg")]
        public string Message { get; set; }
        [JsonIgnore]
        public Results Result { get; set; }

        public Response(){}

        public Response(string msg, Results result)
        {
            Message = msg;
            Result = result;
        }
    }
}