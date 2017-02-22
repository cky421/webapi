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

    public class GroupResult : Group
    {
        public GroupResult()
        {

        }

        public GroupResult(Group group)
        {
            if(group != null)
            {
                GroupId = group.GroupId;
                GroupName = group.GroupName;
                UserId = group.UserId;
            }
        }
        public Result Result { get; set; }
        public string Reason { get; set; } = "None";
    }

    public enum Result
    {
        None,
        Succeed,
        Exists,
        NotExists,
        Failed
    }
}