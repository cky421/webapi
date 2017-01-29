using System;
using TheOne.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace TheOne.Repositories
{
    public class UserRepository: IUserRepository
    {
        private IMongoCollection<User> users;
        public UserRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            users = client.GetDatabase("OneThing").GetCollection<User>("User");

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
            users.InsertOne(new User(){ _id =ObjectId.GenerateNewId(), Username = "admin", Password = "admin"});
        }

        private User FindAdmin()
        {
            return this.Find("admin", "admin");
        }
    }
}