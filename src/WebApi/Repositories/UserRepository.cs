using System;
using WebApi.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using WebApi.Common;

namespace WebApi.Repositories
{
    public class UserRepository: IUserRepository
    {
        private IMongoCollection<User> users;
        public UserRepository()
        {
            var client = new MongoClient(Config.MongoDbConnection);
            users = client.GetDatabase(Config.ApplicationName).GetCollection<User>("User");

            if (FindAdmin() == null)
            {
                InsertAdmin();
            }
        }

        public User Find(String username, String password)
        {
            var builder = Builders<User>.Filter;
            var filter = builder.Eq("Username", username) & builder.Eq("Password", password);
            return users.Find(filter).FirstOrDefault();
        }

        private void InsertAdmin()
        {
            users.InsertOne(new User(){ _id =ObjectId.GenerateNewId(), Username = Config.AdminName, Password = Config.AdminPwd});
        }

        private User FindAdmin()
        {
            return this.Find(Config.AdminName, Config.AdminPwd);
        }
    }
}