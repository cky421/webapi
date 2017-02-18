using MongoDB.Bson;
using MongoDB.Driver;
using WebApi.Common;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly IMongoCollection<User> _users;
        public UserRepository()
        {
            var client = new MongoClient(Config.MongoDbConnection);
            _users = client.GetDatabase(Config.ApplicationName).GetCollection<User>("User");

            if (FindAdmin() == null)
            {
                InsertAdmin();
            }
        }

        public User Find(string username, string password)
        {
            var builder = Builders<User>.Filter;
            var filter = builder.Eq("Username", username) & builder.Eq("Password", password);
            return _users.Find(filter).FirstOrDefault();
        }

        private void InsertAdmin()
        {
            _users.InsertOne(new User { _id =ObjectId.GenerateNewId(), Username = Config.AdminName, Password = Config.AdminPwd});
        }

        private User FindAdmin()
        {
            return Find(Config.AdminName, Config.AdminPwd);
        }
    }
}