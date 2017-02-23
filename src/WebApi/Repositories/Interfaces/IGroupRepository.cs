using System.Collections.Generic;
using WebApi.Models.Mongodb;
using WebApi.Models.QueryResult;

namespace WebApi.Repositories.Interfaces
{
    public interface IGroupRepository
    {
        List<Group> GetAllGroupsByUserId(string userId);
        GroupResult InsertGroup(string groupName, string userId);
        GroupResult UpdateGroup(string newGroupName, string groupId, string userId);
        GroupResult GetGroup(string groupId, string userId);
        GroupResult DeleteGroup(string groupId, string userId);
        void Clear(string userId);
    }
}