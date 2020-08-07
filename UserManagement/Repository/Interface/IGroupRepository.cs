using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Repository.Interface
{
    public interface IGroupRepository
    {
        Task<GroupList> GetAllGroups();
        Task<ActionResult<GetGroupModel>> GetGroupById(Guid Id);
        Task<GroupMember> GetGroupMembersByGroupId(Guid Id);
        Task<GroupMember> GetGroupOwnersByGroupId(Guid Id);
        Task<Group> CreateGroup(GroupModel objGroup);
        Task<bool> DeleteGroup(Guid Id);
    }
}
