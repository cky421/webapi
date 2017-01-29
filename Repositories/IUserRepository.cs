using System;
using TheOne.Models;

namespace TheOne.Repositories
{
    public interface IUserRepository
    {
        User Find(String username, String password);
    }
}