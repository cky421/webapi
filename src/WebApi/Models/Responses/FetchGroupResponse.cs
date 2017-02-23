using System.Collections.Generic;
using Newtonsoft.Json;
using WebApi.Models.Mongodb;

namespace WebApi.Models.Responses
{
    public class FetchGroupResponse : Response
    {
        [JsonProperty("groups")]
        public List<Group> Groups { get; set; }

        public FetchGroupResponse()
        {

        }

        public FetchGroupResponse(List<Group> groups)
        {
            Groups = groups;
        }
    }
}