using Newtonsoft.Json;

namespace WebApi.Models.Responses.Passwords
{
    public class PasswordResponse : Response
    {
        [JsonProperty("passwordid")]
        public string PasswordId { get; set; }
        [JsonProperty("publish")]
        public long Publish { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("pwd")]
        public string Pwd { get; set; }
        [JsonProperty("paypwd")]
        public string PayPwd { get; set; }
        [JsonProperty("groupid")]
        public string GroupId { get; set; }
        [JsonProperty("userid")]
        public string UserId { get; set; }
        [JsonProperty("note")]
        public string Note { get; set; }

        public PasswordResponse(){}

        public PasswordResponse(string msg, Results result, string passwordid, long publish, string title,
            string username, string pwd, string paypwd, string groupid, string userId, string note) : base(msg, result)
        {
            PasswordId = passwordid;
            Publish = publish;
            Username = username;
            Pwd = pwd;
            PayPwd = paypwd;
            GroupId = groupid;
            UserId = userId;
            Note = note;
        }

        public class Builder
        {
            private string _passwordid;
            private long _publish;
            private string _title;
            private string _username;
            private string _pwd;
            private string _paypwd;
            private string _groupid;
            private string _userid;
            private string _note;
            private string _msg;
            private Results _result;

            public Builder SetPasswordId(string passwordid)
            {
                _passwordid = passwordid;
                return this;
            }

            public Builder SetPublish(long publish)
            {
                _publish = publish;
                return this;
            }

            public Builder SetTitle(string title)
            {
                _title = title;
                return this;
            }

            public Builder SetUsername(string username)
            {
                _username = username;
                return this;
            }

            public Builder SetPwd(string pwd)
            {
                _pwd = pwd;
                return this;
            }

            public Builder SetPayPwd(string payPwd)
            {
                _paypwd = payPwd;
                return this;
            }

            public Builder SetGroupId(string groupid)
            {
                _groupid = groupid;
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

            public Builder SetNote(string note)
            {
                _note = note;
                return this;
            }

            public Builder SetResult(Results result)
            {
                _result = result;
                return this;
            }

            public PasswordResponse Build()
            {
                return new PasswordResponse(_msg, _result, _passwordid, _publish, _title, _username, _pwd, _paypwd,
                    _groupid, _userid, _note);
            }
        }
    }
}