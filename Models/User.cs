using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TheOne.Models
{
    public class User 
    {
        public ObjectId _id { get; set; }

        [BsonElement("Username")]
        public string Username { get; set; } 
 
        [BsonElement("Password")]
        public string Password { get; set; } 
    } 
}