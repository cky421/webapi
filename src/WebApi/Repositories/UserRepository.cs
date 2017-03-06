using System;
using MongoDB.Driver;
using WebApi.Common;
using WebApi.Models.Mongodb;
using WebApi.Models.Responses;
using WebApi.Models.Responses.Users;
using WebApi.Repositories.Interfaces;
using static WebApi.Models.Mongodb.Fields;
using static WebApi.Common.Helper;

namespace WebApi.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly IMongoCollection<User> _users;
        public UserRepository()
        {
            var client = new MongoClient(Config.MongoDbConnection);
            _users = client.GetDatabase(Config.ApplicationName).GetCollection<User>("User");

            InsertAdmin();
        }

        public AuthResponse Auth(string userName, string password)
        {
            var existUser = Find(userName, password);
            var builder = new AuthResponse.Builder();
            switch (existUser.Result)
            {
                case Results.NotExists:
                    builder.SetMessage("Username or password is invalid");
                    builder.SetResult(Results.NotExists);
                    break;
                case Results.Succeed:
                    var expiresIn = DateTime.Now + Config.ExpiresSpan;
                    var accessToken = GenerateToken(existUser.UserId, existUser.UserName, expiresIn);
                    builder.SetExpire(Config.ExpiresSpan.TotalSeconds)
                        .SetToken(accessToken)
                        .SetUserId(existUser.UserId)
                        .SetType(Config.TokenType)
                        .SetUsername(existUser.UserName)
                        .SetResult(Results.Succeed);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return builder.Build();
        }

        public UserResponse Find(string userId)
        {
            var filter = Builders<User>.Filter.Eq(UserIdField, userId);
            return Find(filter);
        }

        public UserResponse Find(string username, string password)
        {
            var filter = Builders<User>.Filter.Eq(UsernameField, username) &
                         Builders<User>.Filter.Eq(PasswordField, password);
            return Find(filter);
        }

        public UserResponse Create(string username, string password)
        {
            var filter = Builders<User>.Filter.Eq(UsernameField, username);
            var existUser = _users.Find(filter).FirstOrDefault();
            var builder = new UserResponse.Builder();
            if (existUser != null)
            {
                builder.SetResult(Results.Exists);
                builder.SetMessage($"User with username is {username} exist");
            }
            else
            {
                var user = new User
                {
                    Username = username,
                    Password = password
                };
                _users.InsertOne(user);
                builder.SetResult(Results.Succeed);
                builder.SetUserId(user.UserId);
                builder.SetUserName(user.Username);
            }
            return builder.Build();
        }

        public UserResponse Update(string userId, string password)
        {
            var filter = Builders<User>.Filter.Eq(UserIdField, userId);
            var user = _users.Find(filter).FirstOrDefault();
            var builder = new UserResponse.Builder();
            if (user == null)
            {
                builder.SetResult(Results.NotExists);
                builder.SetMessage("User does not exist");
            }
            else
            {
                var update = Builders<User>.Update.Set(PasswordField, password);
                _users.UpdateOne(filter, update);
                builder.SetResult(Results.Succeed);
                builder.SetUserId(user.UserId);
                builder.SetUserName(user.Username);
            }
            return builder.Build();
        }

        public UserResponse Delete(string userId)
        {
            var filter = Builders<User>.Filter.Eq(UserIdField, userId);
            _users.DeleteOne(filter);
            var builder = new UserResponse.Builder();
            builder.SetResult(Results.Succeed);
            builder.SetUserId(userId);
            return builder.Build();
        }

        private UserResponse Find(FilterDefinition<User> filter)
        {
            var user = _users.Find(filter).FirstOrDefault();
            var builder = new UserResponse.Builder();
            if (user == null)
            {
                builder.SetResult(Results.NotExists);
                builder.SetMessage("User does not exist");
            }
            else
            {
                builder.SetResult(Results.Succeed);
                builder.SetUserId(user.UserId);
                builder.SetUserName(user.Username);
            }
            return builder.Build();
        }

        private void InsertAdmin()
        {
            var filter = Builders<User>.Filter.Eq(UsernameField, Config.AdminName) & Builders<User>.Filter.Eq(PasswordField, Config.AdminPwd);
            var user = _users.Find(filter).FirstOrDefault();
            if (user == null)
            {
                _users.InsertOne(new User{ Username = Config.AdminName, Password = Config.AdminPwd});
            }
        }
    }
}