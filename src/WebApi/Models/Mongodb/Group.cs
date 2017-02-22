using System;
using MongoDB.Bson.Serialization.Attributes;
using static WebApi.Models.Mongodb.Fields;

namespace WebApi.Models.Mongodb
{
    public class Group
    {

        [BsonId]
        public string GroupId { get; set; }
        [BsonElement(GroupNameField)]
        public string GroupName { get; set; }
        [BsonElement(UserIdField)]
        public string UserId { get; set; }

        public Group()
        {
            GroupId = Guid.NewGuid().ToString();
        }
    }

    public class GroupResult
    {
        public Group Group { get; set; }
        public Result Result { get; set; }
        public string Reason { get; set; } = "None";
    }

    public enum Result
    {
        None,
        Succeed,
        Exist,
        Failed
    }
}