using System.Collections.Generic;
using WebApi.Models.Mongodb;

namespace WebApi.Repositories.Interfaces
{
    public interface IGroupRepository
    {
        List<Group> GetAllGroupsByUserId(string userId);
        bool InsertGroup(string groupName, string userId);
        bool UpdateGroup(string newGroupName, string groupId, string userId);
        bool DeleteGroup(string groupId, string userId);
    }
}