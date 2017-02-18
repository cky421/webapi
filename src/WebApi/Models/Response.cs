using System;

namespace WebApi.Models
{
    public class Response
    {
        public ResponseState state { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
    }
}