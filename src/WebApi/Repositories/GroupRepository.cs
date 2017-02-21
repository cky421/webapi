using System.Collections.Generic;
using MongoDB.Driver;
using WebApi.Common;
using WebApi.Models.Mongodb;
using WebApi.Repositories.Interfaces;
using static WebApi.Models.Mongodb.Fields;

namespace WebApi.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly IMongoCollection<Group> _groups;

        public GroupRepository()
        {
            var client = new MongoClient(Config.MongoDbConnection);
            _groups = client.GetDatabase(Config.ApplicationName).GetCollection<Group>("Group");
        }

        public List<Group> GetAllGroupsByUserId(string userId)
        {
            var filter = Builders<Group>.Filter.Eq(UserIdField, userId);
            return _groups.Find(filter).ToList();
        }

        public GroupResult InsertGroup(string groupName, string userId)
        {
            var group = new Group
            {
                GroupName = groupName,
                UserId = userId
            };

            var groupResult = new GroupResult
            {
                Group = group
            };

            if (IsExisted(groupName, userId))
            {
                groupResult.Result = Result.Exist;
            }
            else
            {
                _groups.InsertOne(group);
                groupResult.Result = Result.Succeed;
            }

            return groupResult;
        }

        public GroupResult UpdateGroup(string newGroupName, string groupId, string userId)
        {
            var group = new Group
            {
                GroupId = groupId,
                UserId = userId
            };

            var groupResult = new GroupResult
            {
                Group = group
            };

            if (IsExisted(newGroupName, groupId, userId))
            {
                groupResult.Result = Result.Exist;
            }
            else
            {
                var filter = Builders<Group>.Filter.Eq(GroupIdField, groupId) & Builders<Group>.Filter.Eq(UserIdField, userId);
                var update = Builders<Group>.Update.Set(GroupNameField, newGroupName);
                _groups.UpdateOne(filter, update);
                groupResult.Result = Result.Succeed;
                group.GroupName = newGroupName;
            }
            return groupResult;
        }

        public GroupResult GetGroup(string groupId, string userId)
        {
            var filter = Builders<Group>.Filter.Eq(GroupIdField, groupId) & Builders<Group>.Filter.Eq(UserIdField, userId);
            var group = _groups.Find(filter).FirstOrDefault();
            return new GroupResult
            {
                Group = group,
                Result = Result.Succeed
            };
        }

        public GroupResult DeleteGroup(string groupId, string userId)
        {
            var filter = Builders<Group>.Filter.Eq(GroupIdField, groupId) & Builders<Group>.Filter.Eq(UserIdField, userId);
            _groups.DeleteOne(filter);
            return new GroupResult
            {
                Group = new Group
                {
                    GroupId = groupId,
                    UserId = userId
                },
                Result = Result.Succeed
            };
        }

        public bool IsExisted(string groupName, string userId)
        {
            var filter = Builders<Group>.Filter.Eq(UserIdField, userId) & Builders<Group>.Filter.Eq(GroupNameField, groupName);
            var group = _groups.Find(filter).FirstOrDefault();
            return group != null;
        }

        public bool IsExisted(string groupName, string groupId, string userId)
        {
            var filter = Builders<Group>.Filter.Eq(GroupIdField, groupId) & Builders<Group>.Filter.Eq(UserIdField, userId) &
                         Builders<Group>.Filter.Eq(GroupNameField, groupName);
            var group = _groups.Find(filter).FirstOrDefault();
            return group != null;
        }
    }
}