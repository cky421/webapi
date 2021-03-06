﻿using Newtonsoft.Json;

namespace WebApi.Models.Requests.Groups
{
    public class UpdateGroupRequest
    {
        [JsonProperty("newgroupname")]
        public string NewGroupName { get; set; }
        [JsonProperty("groupid")]
        public string GroupId { get; set; }
    }
}