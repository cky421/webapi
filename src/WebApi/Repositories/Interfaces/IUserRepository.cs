using WebApi.Models.Mongodb;

namespace WebApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User Find(string username, string password);
    }
}