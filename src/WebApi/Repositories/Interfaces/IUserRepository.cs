using WebApi.Models.Responses.Users;

namespace WebApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        AuthResponse Auth(string userName, string password);
        UserResponse Find(string userId);
        UserResponse Find(string userName, string password);
        UserResponse Create(string username, string password);
        UserResponse Update(string userId, string password);
        UserResponse Delete(string userId);
    }
}