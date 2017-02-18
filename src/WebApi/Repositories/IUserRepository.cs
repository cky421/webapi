using WebApi.Models;

namespace WebApi.Repositories
{
    public interface IUserRepository
    {
        User Find(string username, string password);
    }
}