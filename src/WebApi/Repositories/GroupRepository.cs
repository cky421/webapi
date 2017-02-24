using System;
using System.Collections.Generic;
using MongoDB.Driver;
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

        public FetchGroupResponse GetAllGroupsByUserId(string userId)
        {
            var filter = Builders<Group>.Filter.Eq(UserIdField, userId);
            var groups = _groups.Find(filter).ToList();
            var response = new FetchGroupResponse
            {
                Groups = groups,
                Result = Results.Succeed
            };
            return response;
        }

        public GroupResponse InsertGroup(string groupName, string userId)
        {
            CheckUserId(userId);

            var builder = new GroupResponse.Builder().
                SetGroupName(groupName).
                SetUserId(userId);

            var filter = Builders<Group>.Filter.Eq(UserIdField, userId);
            if (_groups.Count(filter) >= Max)
            {
                builder.SetResult(Results.Failed);
                builder.SetMessage($"Maximum number of groups reached：{Max}");
            }
            else if (!string.IsNullOrEmpty(groupName))
            {
                if (IsExisted(groupName, userId))
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
            }
            else
            {
                builder.SetResult(Results.Failed);
                builder.SetMessage($"{nameof(groupName)} is null");
            }
            return builder.Build();
        }

        public GroupResponse UpdateGroup(string newGroupName, string groupId, string userId)
        {
            CheckUserId(userId);

            var builder = new GroupResponse.Builder().
                SetGroupId(groupId).
                SetUserId(userId);


            if (!string.IsNullOrEmpty(newGroupName) && !string.IsNullOrEmpty(groupId))
            {
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
            }
            else
            {
                builder.SetResult(Results.Failed);
                builder.SetMessage($"{nameof(newGroupName)} or/and {nameof(userId)} is/are null");
            }
            return builder.Build();
        }

        public GroupResponse GetGroup(string groupId, string userId)
        {
            CheckUserId(userId);

            var builder = new GroupResponse.Builder();
            if (!string.IsNullOrEmpty(groupId))
            {
                var filter = Builders<Group>.Filter.Eq(GroupIdField, groupId) & Builders<Group>.Filter.Eq(UserIdField, userId);
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
            }
            else
            {
                builder.SetResult(Results.Failed).
                    SetMessage($"{nameof(groupId)} is null").
                    SetUserId(userId);
            }
            return builder.Build();
        }

        public GroupResponse DeleteGroup(string groupId, string userId)
        {
            CheckUserId(userId);

            var builder = new GroupResponse.Builder();
            if (!string.IsNullOrEmpty(groupId))
            {
                var queryResult = GetGroup(groupId, userId);
                if (queryResult.Result == Results.Succeed)
                {
                    var filter = Builders<Group>.Filter.Eq(GroupIdField, groupId) & Builders<Group>.Filter.Eq(UserIdField, userId);
                    _groups.DeleteOne(filter);
                }
                builder.SetGroupId(queryResult.GroupId)
                    .SetGroupName(queryResult.GroupName)
                    .SetUserId(queryResult.UserId)
                    .SetMessage(queryResult.Message)
                    .SetResult(queryResult.Result);
            }
            else
            {
                builder.SetGroupId($"{nameof(groupId)} is null")
                    .SetUserId(userId)
                    .SetResult(Results.Failed);
            }
            return builder.Build();
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