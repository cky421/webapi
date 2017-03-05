using Newtonsoft.Json;

namespace WebApi.Models.Responses.Users
{
    public class AuthResponse : Response
    {
        [JsonProperty("expire")]
        public double Expire { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("userid")]
        public string UserId { get; set; }

        public AuthResponse(){}

        public AuthResponse(string msg, Results result, double expire, string type, string token, string username,
            string userid) : base(msg, result)
        {
            Expire = expire;
            Type = type;
            Token = token;
            Username = username;
            UserId = userid;
        }

        public new class Builder
        {
            private double _expire;
            private string _type;
            private string _token;
            private string _username;
            private string _userid;
            private string _msg;
            private Results _result;

            public Builder SetExpire(double expire)
            {
                _expire = expire;
                return this;
            }

            public Builder SetType(string type)
            {
                _type = type;
                return this;
            }

            public Builder SetToken(string token)
            {
                _token = token;
                return this;
            }

            public Builder SetUsername(string username)
            {
                _username = username;
                return this;
            }

            public Builder SetUserId(string userid)
            {
                _userid = userid;
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

            public AuthResponse Build()
            {
                return new AuthResponse(_msg, _result, _expire, _type, _token, _username, _userid);
            }
        }
    }
}