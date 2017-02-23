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
            var filter = Builders<Group>.Filter.Eq(UserIdField, userId);
            return _groups.Find(filter).ToList();
        }

        public GroupResult InsertGroup(string groupName, string userId)
        {
            CheckUserId(userId);

            var groupResult = new GroupResult(new Group
            {
                GroupName = groupName,
                UserId = userId
            });

            if (!string.IsNullOrEmpty(groupName))
            {
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
            }
            else
            {
                groupResult.Result = Result.Failed;
                groupResult.Reason = $"{nameof(groupName)} is null";
            }


            return groupResult;
        }

        public GroupResult UpdateGroup(string newGroupName, string groupId, string userId)
        {
            CheckUserId(userId);

            var groupResult = new GroupResult(new Group
            {
                GroupId = groupId,
                UserId = userId
            });

            if (!string.IsNullOrEmpty(newGroupName) && !string.IsNullOrEmpty(groupId))
            {
                if (IsExisted(newGroupName, groupId, userId))
                {
                    groupResult.Result = Result.Exists;
                    groupResult.Reason = $"{newGroupName} is existed";
                }
                else
                {
                    var filter = Builders<Group>.Filter.Eq(GroupIdField, groupId) &
                                 Builders<Group>.Filter.Eq(UserIdField, userId);
                    var update = Builders<Group>.Update.Set(GroupNameField, newGroupName);
                    _groups.UpdateOne(filter, update);
                    groupResult.Result = Result.Succeed;
                    groupResult.GroupName = newGroupName;
                }
            }
            else
            {
                groupResult.Result = Result.Failed;
                groupResult.Reason = $"{nameof(newGroupName)} or/and {nameof(userId)} is/are null";
            }
            return groupResult;
        }

        public GroupResult GetGroup(string groupId, string userId)
        {
            CheckUserId(userId);

            GroupResult groupResult;
            if (!string.IsNullOrEmpty(groupId))
            {
                var filter = Builders<Group>.Filter.Eq(GroupIdField, groupId) & Builders<Group>.Filter.Eq(UserIdField, userId);
                var group = _groups.Find(filter).FirstOrDefault();
                groupResult = group != null ? new GroupResult(group) {Result = Result.Succeed} :
                    new GroupResult{ Result = Result.NotExists, Reason = "Can not find such group"};
            }
            else
            {
                groupResult = new GroupResult
                {
                    UserId = userId,
                    Reason = $"{nameof(groupId)} is null",
                    Result = Result.Failed
                };
            }


            return groupResult;
        }

        public GroupResult DeleteGroup(string groupId, string userId)
        {
            CheckUserId(userId);

            GroupResult groupResult;
            if (!string.IsNullOrEmpty(groupId))
            {
                var queryResult = GetGroup(groupId, userId);
                if (queryResult.Result == Result.Succeed)
                {
                    var filter = Builders<Group>.Filter.Eq(GroupIdField, groupId) & Builders<Group>.Filter.Eq(UserIdField, userId);
                    _groups.DeleteOne(filter);
                    groupResult = queryResult;
                }
                else
                {
                    groupResult = queryResult;
                }
            }
            else
            {
                groupResult = new GroupResult
                {
                    UserId = userId,
                    Reason = $"{nameof(groupId)} is null",
                    Result = Result.Failed
                };
            }
            return groupResult;
        }

        public void Clear(string userId)
        {
            CheckUserId(userId);

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

        private static void CheckUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException($"{nameof(userId)} is null");
            }
        }
    }
}