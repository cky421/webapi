using System;
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
            NullCheck(userId, nameof(userId));

            var filter = Builders<Group>.Filter.Eq(UserIdField, userId);
            return _groups.Find(filter).ToList();
        }

        public GroupResult InsertGroup(string groupName, string userId)
        {
            NullCheck(groupName, nameof(groupName));
            NullCheck(userId, nameof(userId));

            var groupResult = new GroupResult(new Group
            {
                GroupName = groupName,
                UserId = userId
            });

            if (IsExisted(groupName, userId))
            {
                groupResult.Result = Result.Exists;
                groupResult.Reason = $"{groupName} is existed";
            }
            else
            {
                _groups.InsertOne(groupResult);
                groupResult.Result = Result.Succeed;
            }

            return groupResult;
        }

        public GroupResult UpdateGroup(string newGroupName, string groupId, string userId)
        {
            NullCheck(newGroupName, nameof(newGroupName));
            NullCheck(groupId, nameof(groupId));
            NullCheck(userId, nameof(userId));

            var groupResult = new GroupResult(new Group
            {
                GroupId = groupId,
                UserId = userId
            });

            if (IsExisted(newGroupName, groupId, userId))
            {
                groupResult.Result = Result.Exists;
                groupResult.Reason = $"{newGroupName} is existed";
            }
            else
            {
                var filter = Builders<Group>.Filter.Eq(GroupIdField, groupId) & Builders<Group>.Filter.Eq(UserIdField, userId);
                var update = Builders<Group>.Update.Set(GroupNameField, newGroupName);
                _groups.UpdateOne(filter, update);
                groupResult.Result = Result.Succeed;
                groupResult.GroupName = newGroupName;
            }
            return groupResult;
        }

        public GroupResult GetGroup(string groupId, string userId)
        {
            NullCheck(groupId, nameof(groupId));
            NullCheck(userId, nameof(userId));

            var filter = Builders<Group>.Filter.Eq(GroupIdField, groupId) & Builders<Group>.Filter.Eq(UserIdField, userId);
            var group = _groups.Find(filter).FirstOrDefault();
            return group != null ? new GroupResult(group) {Result = Result.Succeed} :
                new GroupResult{ Result = Result.Failed, Reason = "Can not find such group"};
        }

        public GroupResult DeleteGroup(string groupId, string userId)
        {
            NullCheck(groupId, nameof(groupId));
            NullCheck(userId, nameof(userId));

            var queryResult = GetGroup(groupId, userId);
            GroupResult groupResult;
            if (queryResult.Result == Result.Exists)
            {
                var filter = Builders<Group>.Filter.Eq(GroupIdField, groupId) & Builders<Group>.Filter.Eq(UserIdField, userId);
                _groups.DeleteOne(filter);
                groupResult = queryResult;
            }
            else
            {
                groupResult = new GroupResult(new Group{GroupId = groupId, UserId = userId})
                {
                    Result = Result.NotExists,
                    Reason = "Can not find such group"
                };
            }

            return groupResult;
        }

        public void Clear(string userId)
        {
            NullCheck(userId, nameof(userId));

            var filter = Builders<Group>.Filter.Eq(UserIdField, userId);
            _groups.DeleteMany(filter);
        }

        public bool IsExisted(string groupName, string userId)
        {
            NullCheck(groupName, nameof(groupName));
            NullCheck(userId, nameof(userId));

            var filter = Builders<Group>.Filter.Eq(UserIdField, userId) & Builders<Group>.Filter.Eq(GroupNameField, groupName);
            var group = _groups.Find(filter).FirstOrDefault();
            return group != null;
        }

        public bool IsExisted(string groupName, string groupId, string userId)
        {
            NullCheck(groupName, nameof(groupName));
            NullCheck(groupId, nameof(groupId));
            NullCheck(userId, nameof(userId));

            var filter = Builders<Group>.Filter.Eq(GroupIdField, groupId) & Builders<Group>.Filter.Eq(UserIdField, userId) &
                         Builders<Group>.Filter.Eq(GroupNameField, groupName);
            var group = _groups.Find(filter).FirstOrDefault();
            return group != null;
        }

        private static void NullCheck(string filed, string filedName)
        {
            if (string.IsNullOrEmpty(filed))
            {
                throw new NullReferenceException($"{filedName} is null");
            }
        }
    }
}