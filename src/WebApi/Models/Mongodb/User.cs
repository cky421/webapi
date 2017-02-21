using System;
using MongoDB.Bson.Serialization.Attributes;
using static WebApi.Models.Mongodb.Fields;

namespace WebApi.Models.Mongodb
{
    public class User
    {
        [BsonId]
        public string UserId { get; set; }
        [BsonElement(UsernameField)]
        public string Username { get; set; }
        [BsonElement(PasswordField)]
        public string Password { get; set; }

        public User()
        {
            UserId = Guid.NewGuid().ToString();
        }

    } 
}