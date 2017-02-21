using System;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models.Mongodb
{
    public class User 
    {
        [BsonId]
        public string UserId { get; set; }
        [BsonElement("Username")]
        public string Username { get; set; }
        [BsonElement("Password")]
        public string Password { get; set; }

        public User()
        {
            UserId = Guid.NewGuid().ToString();
        }

    } 
}