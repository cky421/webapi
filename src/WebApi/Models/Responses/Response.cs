using Newtonsoft.Json;

namespace WebApi.Models.Responses
{
    public class Response
    {
        [JsonProperty("msg")]
        public string Message { get; set; }
        [JsonIgnore]
        public Results Result { get; set; }

        public Response(){}

        public Response(string msg, Results result)
        {
            Message = msg;
            Result = result;
        }

        public class Builder
        {
            protected string Msg;
            protected Results Result;

            public virtual Builder SetMessage(string msg)
            {
                Msg = msg;
                return this;
            }

            public virtual Builder SetResult(Results result)
            {
                Result = result;
                return this;
            }

            public Response Build()
            {
                return new Response(Msg, Result);
            }
        }

    }
}