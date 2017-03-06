using Newtonsoft.Json;

namespace WebApi.Models.Responses.Groups
{
    public class GroupResponse : Response
    {
        [JsonProperty("groupname")]
        public string GroupName { get; set; }
        [JsonProperty("groupid")]
        public string GroupId { get; set; }
        [JsonProperty("userid")]
        public string UserId { get; set; }

        public GroupResponse(){}

        public GroupResponse(string msg, Results result, string groupName, string groupId, string userId) : base(msg, result)
        {
            GroupName = groupName;
            GroupId = groupId;
            UserId = userId;
        }

        public new class Builder
        {
            private string _groupId;
            private string _userId;
            private string _groupName;
            private string _msg;
            private Results _result;

            public Builder SetGroupId(string groupId)
            {
                _groupId = groupId;
                return this;
            }

            public Builder SetUserId(string userId)
            {
                _userId = userId;
                return this;
            }

            public Builder SetGroupName(string groupName)
            {
                _groupName = groupName;
                return this;
            }

            public Builder SetMessage(string msg)
            {
                _msg = msg;
                return this;
            }

            public Builder SetResult(Results result)
            {
                _result = result;
                return this;
            }

            public GroupResponse Build()
            {
                return new GroupResponse(_msg, _result, _groupName, _groupId, _userId);
            }
        }
    }
}