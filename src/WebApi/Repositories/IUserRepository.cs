using System;
using WebApi.Models;

namespace WebApi.Repositories
{
    public interface IUserRepository
    {
        User Find(String username, String password);
    }
}