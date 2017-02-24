using WebApi.Models.Responses.Groups;

namespace WebApi.Repositories.Interfaces
{
    public interface IGroupRepository
    {
        FetchGroupResponse GetAllGroupsByUserId(string userId);
        GroupResponse InsertGroup(string groupName, string userId);
        GroupResponse UpdateGroup(string newGroupName, string groupId, string userId);
        GroupResponse GetGroup(string groupId, string userId);
        GroupResponse DeleteGroup(string groupId, string userId);
        void Clear(string userId);
    }
}