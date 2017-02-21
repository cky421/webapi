using System.Collections.Generic;
using WebApi.Models.Mongodb;
using WebApi.Repositories.Interfaces;

namespace WebApi.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        public List<Group> GetAllGroupsByUserId(string userId)
        {
            throw new System.NotImplementedException();
        }

        public bool InsertGroup(string groupName, string userId)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateGroup(string newGroupName, string groupId, string userId)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteGroup(string groupId, string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}