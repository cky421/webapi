using System;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models.Mongodb
{
    public class Group
    {
        [BsonId]
        public string GroupId { get; set; }
        [BsonElement("GroupName")]
        public string GroupName { get; set; }
        [BsonElement("UserId")]
        public string UserId { get; set; }

        public Group()
        {
            GroupId = Guid.NewGuid().ToString();
        }
    }
}