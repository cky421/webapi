using System;
using MongoDB.Driver;
using WebApi.Attributes;
using WebApi.Common;
using WebApi.Models.Mongodb;
using WebApi.Models.Responses;
using WebApi.Models.Responses.Passwords;
using WebApi.Repositories.Interfaces;
using static WebApi.Models.Mongodb.Fields;

namespace WebApi.Repositories
{
    public class PasswordRepository : IPasswordRepository
    {
        private readonly IMongoCollection<Group> _groups;
        private readonly IMongoCollection<Password> _passwords;
        private const long Max = 25;

        public PasswordRepository()
        {
            var client = new MongoClient(Config.MongoDbConnection);
            _passwords = client.GetDatabase(Config.ApplicationName).GetCollection<Password>("Password");
            _groups = client.GetDatabase(Config.ApplicationName).GetCollection<Group>("Group");
        }

        public FetchPasswordsResponse GetAllPasswordByGroupId([NotNullOrWhiteSpace]string groupId, [NotNullOrWhiteSpace]string userId)
        {
            var filter = Builders<Password>.Filter.Eq(UserIdField, userId) &
                         Builders<Password>.Filter.Eq(GroupIdField, groupId);
            var passwords = _passwords.Find(filter).ToList();
            return new FetchPasswordsResponse
            {
                Result = Results.Succeed,
                Passwords = passwords
            };
        }

        public PasswordResponse InsertPassword([NotNullOrWhiteSpace]string groupId,
                                            [NotNullOrWhiteSpace]string userId,
                                            [NotNullOrWhiteSpace]string title,
                                            string username, string pwd,
                                            string paypwd, string note)
        {

            var builder = new PasswordResponse.Builder();

            var filter = Builders<Password>.Filter.Eq(UserIdField, userId) &
                         Builders<Password>.Filter.Eq(GroupIdField, groupId);

            if (_passwords.Count(filter) >= Max)
            {
                builder.SetResult(Results.Failed);
                builder.SetMessage($"Maximum number of groups reached：{Max}");
            } else if (!IsGroupExisted(groupId, userId))
            {
                builder.SetResult(Results.Failed);
                builder.SetMessage($"group with id {groupId} doesn't exist");
            }
            else
            {
                var publish = DateTime.UtcNow.Ticks;
                var password = new Password
                {
                    GroupId = groupId,
                    Note = note,
                    PayPwd = paypwd,
                    Publish = publish,
                    Pwd = pwd,
                    Title = title,
                    UserId = userId,
                    Username = username
                };

                _passwords.InsertOne(password);

                builder.SetUserId(userId)
                    .SetGroupId(groupId)
                    .SetTitle(title)
                    .SetUsername(username)
                    .SetPwd(pwd)
                    .SetPayPwd(paypwd)
                    .SetNote(note)
                    .SetPublish(publish)
                    .SetPasswordId(password.PasswordId)
                    .SetResult(Results.Succeed);
            }

            return builder.Build();
        }

        public PasswordResponse UpdatePassword([NotNullOrWhiteSpace]string groupId,
                                            [NotNullOrWhiteSpace]string userId,
                                            [NotNullOrWhiteSpace]string passwordId,
                                            [NotNullOrWhiteSpace]string title,
                                            string username, string pwd,
                                            string paypwd, string note)
        {
            var builder = new PasswordResponse.Builder();

            if(!IsGroupExisted(groupId, userId))
            {
                builder.SetResult(Results.Failed);
                builder.SetMessage($"group with id {groupId} doesn't exist");
            }
            else if (IsExisted(passwordId, userId))
            {
                var publish = DateTime.UtcNow.Ticks;
                var filter = Builders<Password>.Filter.Eq(PassworIdField, passwordId) &
                             Builders<Password>.Filter.Eq(UserIdField, userId);
                var update = Builders<Password>.Update.Set(TitleField, title)
                    .Set(UsernameField, username)
                    .Set(PwdField, pwd)
                    .Set(PayPwdField, paypwd)
                    .Set(PublishField, publish)
                    .Set(NoteField, note)
                    .Set(GroupIdField, groupId);
                _passwords.UpdateOne(filter, update);

                builder.SetUserId(userId)
                    .SetGroupId(groupId)
                    .SetTitle(title)
                    .SetUsername(username)
                    .SetPwd(pwd)
                    .SetPayPwd(paypwd)
                    .SetNote(note)
                    .SetPublish(publish)
                    .SetPasswordId(passwordId)
                    .SetResult(Results.Succeed);
            }
            else
            {
                builder.SetResult(Results.NotExists);
                builder.SetMessage("password doesn't  exisit");
            }

            return builder.Build();
        }

        public PasswordResponse GetPassword([NotNullOrWhiteSpace]string userId, [NotNullOrWhiteSpace]string passwordId)
        {
            var builder = new PasswordResponse.Builder();

            var filter = Builders<Password>.Filter.Eq(PassworIdField, passwordId) &
                         Builders<Password>.Filter.Eq(UserIdField, userId);
            var password = _passwords.Find(filter).FirstOrDefault();
            if (password != null)
            {
                builder.SetUserId(password.UserId)
                    .SetGroupId(password.GroupId)
                    .SetTitle(password.Title)
                    .SetUsername(password.Username)
                    .SetPwd(password.Pwd)
                    .SetPayPwd(password.PayPwd)
                    .SetNote(password.Note)
                    .SetPublish(password.Publish)
                    .SetPasswordId(password.PasswordId)
                    .SetResult(Results.Succeed);
            }
            else
            {
                builder.SetResult(Results.NotExists).
                    SetMessage("Can not find such password");
            }

            return builder.Build();
        }

        public PasswordResponse DeletePassword([NotNullOrWhiteSpace]string userId, [NotNullOrWhiteSpace]string passwordId)
        {
            var builder = new PasswordResponse.Builder();

            var queryResult = GetPassword(userId, passwordId);
            if (queryResult.Result == Results.Succeed)
            {
                var filter = Builders<Password>.Filter.Eq(PassworIdField, passwordId) & Builders<Password>.Filter.Eq(UserIdField, userId);
                _passwords.DeleteOne(filter);
            }
            builder.SetUserId(queryResult.UserId)
                .SetGroupId(queryResult.GroupId)
                .SetTitle(queryResult.Title)
                .SetUsername(queryResult.Username)
                .SetPwd(queryResult.Pwd)
                .SetPayPwd(queryResult.PayPwd)
                .SetNote(queryResult.Note)
                .SetPublish(queryResult.Publish)
                .SetPasswordId(queryResult.PasswordId)
                .SetResult(Results.Succeed);

            return builder.Build();
        }

        public void Clear(string userId)
        {
            var filter = Builders<Password>.Filter.Eq(UserIdField, userId);
            _passwords.DeleteMany(filter);
        }

        private bool IsExisted(string passwordId, string userId)
        {
            var filter = Builders<Password>.Filter.Eq(PassworIdField, passwordId) &
                         Builders<Password>.Filter.Eq(UserIdField, userId);
            var password = _passwords.Find(filter).FirstOrDefault();
            return password != null;
        }

        private bool IsGroupExisted(string groupId, string userId)
        {
            var filter = Builders<Group>.Filter.Eq(GroupIdField, groupId) &
                         Builders<Group>.Filter.Eq(UserIdField, userId);
            var group = _groups.Find(filter).FirstOrDefault();
            return group != null;
        }
    }
}