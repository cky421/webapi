using WebApi.Models.Responses.Passwords;

namespace WebApi.Repositories.Interfaces
{
    public interface IPasswordRepository
    {
        FetchPasswordsResponse GetAllPasswordByGroupId(string groupId, string userId);
        PasswordResponse InsertPassword(string groupId, string userId, string title, string username, string pwd,
            string paypwd, string note);
        PasswordResponse UpdatePassword(string groupId, string userId, string passwordId, string title, string username,
            string pwd, string paypwd, string note);
        PasswordResponse GetPassword(string userId, string passwordId);
        PasswordResponse DeletePassword(string userId, string passwordId);
        void Clear(string userId);
    }
}