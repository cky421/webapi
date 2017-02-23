namespace WebApi.Models.QueryResult
{
    public class GroupResult
    {
        public GroupResult(string groupId, string userId, string groupName, Results result, string reason)
        {
            GroupId = groupId;
            UserId = userId;
            GroupName = groupName;
            Result = result;
            Reason = reason;
        }

        public string GroupId { get; }
        public string UserId { get; }
        public string GroupName { get; }
        public Results Result { get; }
        public string Reason { get; }

        public class GroupResultBuilder
        {
            private string _groupId;
            private string _userId;
            private string _groupName;
            private Results _result;
            private string _reason = "None";

            public GroupResultBuilder SetGroupId(string groupId)
            {
                _groupId = groupId;
                return this;
            }

            public GroupResultBuilder SetUserId(string userId)
            {
                _userId = userId;
                return this;
            }

            public GroupResultBuilder SetGroupName(string groupName)
            {
                _groupName = groupName;
                return this;
            }

            public GroupResultBuilder SetResult(Results result)
            {
                _result = result;
                return this;
            }

            public GroupResultBuilder SetReason(string reason)
            {
                _reason = reason;
                return this;
            }

            public GroupResult Builder()
            {
                return new GroupResult(_groupId, _userId, _groupName, _result, _reason);
            }

        }
    }
}