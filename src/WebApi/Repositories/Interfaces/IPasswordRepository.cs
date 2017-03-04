using WebApi.Models.Responses.Passwords;

namespace WebApi.Repositories.Interfaces
{
    public interface IPasswordRepository
    {
        FetchPasswordsResponse GetAllPasswordByGroupId(string groupId, string userId);
        PasswordResponse InsertPassword(string userId, string title, string username, string pwd, string paypwd, string groupId, string note);
        PasswordResponse UpdatePassword(string passwordId, string userId, string title, string username, string pwd, string paypwd, string groupId, string note);
        PasswordResponse GetPassword(string passwordId, string userId);
        PasswordResponse DeletePassword(string passwordId, string userId);
        void Clear(string userId);
    }
}