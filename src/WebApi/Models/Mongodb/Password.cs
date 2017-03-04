using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using static WebApi.Models.Mongodb.Fields;

namespace WebApi.Models.Mongodb
{
    public class Password
    {
        [BsonId]
        [JsonProperty("passwordid")]
        public string PasswordId { get; set; }
        [BsonElement(PublishField)]
        [JsonProperty("publish")]
        public long Publish { get; set; }
        [BsonElement(TitleField)]
        [JsonProperty("title")]
        public string Title { get; set; }
        [BsonElement(UsernameField)]
        [JsonProperty("username")]
        public string Username { get; set; }
        [BsonElement(PwdField)]
        [JsonProperty("pwd")]
        public string Pwd { get; set; }
        [BsonElement(PayPwdField)]
        [JsonProperty("paypwd")]
        public string PayPwd { get; set; }
        [BsonElement(GroupIdField)]
        [JsonProperty("groupid")]
        public string GroupId { get; set; }
        [BsonElement(UserIdField)]
        [JsonProperty("userid")]
        public string UserId { get; set; }
        [BsonElement(NoteField)]
        [JsonProperty("note")]
        public string Note { get; set; }

        public Password()
        {
            PasswordId = Guid.NewGuid().ToString();
        }
    }
}