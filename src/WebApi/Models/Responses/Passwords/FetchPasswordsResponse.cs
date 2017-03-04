using System.Collections.Generic;
using Newtonsoft.Json;
using WebApi.Models.Mongodb;

namespace WebApi.Models.Responses.Passwords
{
    public class FetchPasswordsResponse : Response
    {
        [JsonProperty("passwords")]
        public List<Password> Passwords { get; set; }
    }
}