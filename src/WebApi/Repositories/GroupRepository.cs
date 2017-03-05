using MongoDB.Driver;
using WebApi.Attributes;
using WebApi.Common;
using WebApi.Models.Mongodb;
using WebApi.Models.Responses;
using WebApi.Models.Responses.Groups;
using WebApi.Repositories.Interfaces;
using static WebApi.Models.Mongodb.Fields;

namespace WebApi.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly IMongoCollection<Group> _groups;
        private const long Max = 25;

        public GroupRepository()
        {
            var client = new MongoClient(Config.MongoDbConnection);
            _groups = client.GetDatabase(Config.ApplicationName).GetCollection<Group>("Group");
        }

        public FetchGroupResponse GetAllGroupsByUserId([NotNullOrWhiteSpace]string userId)
        {
            var filter = Builders<Group>.Filter.Eq(UserIdField, userId);
            var groups = _groups.Find(filter).ToList();
            if (groups.Count == 0)
            {
                var group = InsertGroup("Default", userId);
                groups.Add(new Group
                {
                    GroupId = group.GroupId,
                    GroupName = group.GroupName,
                    UserId = group.UserId
                });
            }

            var response = new FetchGroupResponse
            {
                Groups = groups,
                Result = Results.Succeed
            };
            return response;
        }

        public GroupResponse InsertGroup([NotNullOrWhiteSpace]string userId, [NotNullOrWhiteSpace]string groupName)
        {
            var builder = new GroupResponse.Builder().SetGroupName(groupName).SetUserId(userId);

            var filter = Builders<Group>.Filter.Eq(UserIdField, userId);
            if (_groups.Count(filter) >= Max)
            {
                builder.SetResult(Results.Failed);
                builder.SetMessage($"Maximum number of groups reached：{Max}");
            } else if (IsExisted(groupName, userId))
            {
                builder.SetResult(Results.Exists);
                builder.SetMessage($"{groupName} is existed");
            }
            else
            {
                var group = new Group
                {
                    GroupName = groupName,
                    UserId = userId
                };
                _groups.InsertOne(group);

                builder.SetResult(Results.Succeed).SetGroupId(group.GroupId);
            }

            return builder.Build();
        }

        public GroupResponse UpdateGroup([NotNullOrWhiteSpace]string groupId,
            [NotNullOrWhiteSpace]string userId,
            [NotNullOrWhiteSpace]string newGroupName)
        {
            var builder = new GroupResponse.Builder().SetGroupId(groupId).SetUserId(userId);

            if (IsExisted(newGroupName, groupId, userId))
            {
                builder.SetResult(Results.Exists);
                builder.SetMessage($"{newGroupName} is existed");
            }
            else
            {
                var filter = Builders<Group>.Filter.Eq(GroupIdField, groupId) &
                             Builders<Group>.Filter.Eq(UserIdField, userId);
                var update = Builders<Group>.Update.Set(GroupNameField, newGroupName);
                _groups.UpdateOne(filter, update);
                builder.SetResult(Results.Succeed);
                builder.SetGroupName(newGroupName);
            }

            return builder.Build();
        }

        public GroupResponse GetGroup([NotNullOrWhiteSpace]string groupId, [NotNullOrWhiteSpace]string userId)
        {
            var builder = new GroupResponse.Builder();

            var filter = Builders<Group>.Filter.Eq(GroupIdField, groupId) &
                         Builders<Group>.Filter.Eq(UserIdField, userId);
            var group = _groups.Find(filter).FirstOrDefault();
            if (group != null)
            {
                builder.SetGroupId(group.GroupId)
                    .SetGroupName(group.GroupName)
                    .SetUserId(group.UserId)
                    .SetResult(Results.Succeed);
            }
            else
            {
                builder.SetResult(Results.NotExists).
                    SetMessage("Can not find such group");
            }

            return builder.Build();
        }

        public GroupResponse DeleteGroup([NotNullOrWhiteSpace]string groupId, [NotNullOrWhiteSpace]string userId)
        {
            var builder = new GroupResponse.Builder();

            var queryResult = GetGroup(groupId, userId);
            if (queryResult.Result == Results.Succeed)
            {
                var filter = Builders<Group>.Filter.Eq(GroupIdField, groupId) &
                             Builders<Group>.Filter.Eq(UserIdField, userId);
                _groups.DeleteOne(filter);
            }
            builder.SetGroupId(queryResult.GroupId)
                .SetGroupName(queryResult.GroupName)
                .SetUserId(queryResult.UserId)
                .SetMessage(queryResult.Message)
                .SetResult(queryResult.Result);

            return builder.Build();
        }

        public void Clear([NotNullOrWhiteSpace]string userId)
        {
            var filter = Builders<Group>.Filter.Eq(UserIdField, userId);
            _groups.DeleteMany(filter);
        }

        private bool IsExisted(string groupName, string userId)
        {
            var filter = Builders<Group>.Filter.Eq(UserIdField, userId) & Builders<Group>.Filter.Eq(GroupNameField, groupName);
            var group = _groups.Find(filter).FirstOrDefault();
            return group != null;
        }

        private bool IsExisted(string groupName, string groupId, string userId)
        {
            var filter = Builders<Group>.Filter.Eq(GroupIdField, groupId) & Builders<Group>.Filter.Eq(UserIdField, userId) &
                         Builders<Group>.Filter.Eq(GroupNameField, groupName);
            var group = _groups.Find(filter).FirstOrDefault();
            return group != null;
        }
    }
}