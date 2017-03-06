using Newtonsoft.Json;

namespace WebApi.Models.Responses.Users
{
    public class UserResponse: Response
    {
        [JsonProperty("userid")]
        public string UserId { get; set; }
        [JsonProperty("username")]
        public string UserName { get; set; }

        public UserResponse(){}

        public UserResponse(string msg, Results result, string userId, string userName) : base(msg, result)
        {
            UserId = userId;
            UserName = userName;
        }

        public new class Builder
        {
            private string _userId;
            private string _userName;
            private string _msg;
            private Results _result;

            public Builder SetUserId(string userId)
            {
                _userId = userId;
                return this;
            }

            public Builder SetUserName(string userName)
            {
                _userName = userName;
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

            public UserResponse Build()
            {
                return new UserResponse(_msg, _result, _userId, _userName);
            }
        }
    }
}