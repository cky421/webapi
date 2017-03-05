using WebApi.Models.Responses.Groups;

namespace WebApi.Repositories.Interfaces
{
    public interface IGroupRepository
    {
        FetchGroupResponse GetAllGroupsByUserId(string userId);
        GroupResponse InsertGroup(string userId, string groupName);
        GroupResponse UpdateGroup(string groupId, string userId, string newGroupName);
        GroupResponse GetGroup(string groupId, string userId);
        GroupResponse DeleteGroup(string groupId, string userId);
        void Clear(string userId);
    }
}