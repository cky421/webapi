using System;

namespace TheOne.Models
{
    public class Response
    {
        public RequestState State { get; set; }
        public string Msg { get; set; }
        public Object Data { get; set; }
    }
}