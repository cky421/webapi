using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using static WebApi.Models.Mongodb.Fields;

namespace WebApi.Models.Mongodb
{
    public class Group
    {

        [BsonId]
        [JsonProperty("groupid")]
        public string GroupId { get; set; }
        [BsonElement(GroupNameField)]
        [JsonProperty("groupname")]
        public string GroupName { get; set; }
        [BsonElement(UserIdField)]
        [JsonProperty("userid")]
        public string UserId { get; set; }

        public Group()
        {
            GroupId = Guid.NewGuid().ToString();
        }
    }
}