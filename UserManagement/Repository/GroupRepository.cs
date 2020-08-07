using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Repository.Interface;
using UserManagement.Services.Interfaces;

namespace UserManagement.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly IDataHandler _dataHandler;
        private readonly IGraphClient _graphClient;

        public GroupRepository(IDataHandler dataHandler, IGraphClient graphClient)
        {
            _dataHandler = dataHandler;
            _graphClient = graphClient;
        }

        public async Task<GroupList> GetAllGroups()
        {
            try
            {
                GroupList ListGroup = new GroupList();
                ListGroup.Groups = new List<GetGroupModel>();
                GraphServiceClient graphClient = await _graphClient.GetServiceClient();
                var groups = await graphClient.Groups
                        .Request()
                        .GetAsync();
                foreach (var eachGroup in groups)
                {
                    var objGroup = _dataHandler.GroupBind(eachGroup);
                    ListGroup.Groups.Add(objGroup);
                }
                ListGroup.totalCount = groups.Count;


                return ListGroup;
            }
            catch (ServiceException)
            {
                return null;
            }
        }

        public async Task<ActionResult<GetGroupModel>> GetGroupById(Guid Id)
        {
            try
            {
                GraphServiceClient graphClient = await _graphClient.GetServiceClient();
                var group = await graphClient.Groups[Id.ToString()]
                                            .Request()
                                            .GetAsync();
                var bindedGroup =  _dataHandler.GroupBind(group);
                return bindedGroup;
            }
            catch (ServiceException)
            {
                return null;
            }

        }

        public async Task<Group> CreateGroup(GroupModel objGroup)
        {
            try
            {

                GraphServiceClient graphClient = await _graphClient.GetServiceClient();
                var group = await _dataHandler.AssignDataToAddGroup(objGroup);
                await graphClient.Groups
                    .Request()
                    .AddAsync(group.Value);
                 return group.Value;

            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex);
                return null;
            }

           

        }

        public async Task<GroupMember> GetGroupMembersByGroupId(Guid Id)
        {
            try
            {
                GroupMember ListMember = new GroupMember();
                ListMember.GroupMembers = new List<UserModel>();
                GraphServiceClient graphClient = await _graphClient.GetServiceClient();
                var members = await graphClient.Groups[Id.ToString()].Members
                                                .Request()
                                                .GetAsync();

                foreach (var eachMember in members.CurrentPage)
                {
                    var myUser = (User)eachMember;
                    var objMember = _dataHandler.UserProperty(myUser);
                    ListMember.GroupMembers.Add(objMember);
                }
                ListMember.totalCount = members.Count;
                return ListMember;

            }
            catch (ServiceException)
            {
                return null;
            }

        }

        public async Task<GroupMember> GetGroupOwnersByGroupId(Guid Id)
        {
            try
            {
                GroupMember ListMember = new GroupMember();
                ListMember.GroupMembers = new List<UserModel>();
                GraphServiceClient graphClient = await _graphClient.GetServiceClient();
                var members = await graphClient.Groups[Id.ToString()].Owners
                                                .Request()
                                                .GetAsync();

                foreach (var eachMember in members.CurrentPage)
                {
                    var myUser = (User)eachMember;
                    var objMember = _dataHandler.UserProperty(myUser);
                    ListMember.GroupMembers.Add(objMember);
                }
                ListMember.totalCount = members.Count;
                return ListMember;

            }
            catch (ServiceException)
            {
                return null;
            }

        }

        public async Task<bool> DeleteGroup(Guid Id)
        {
            try
            {
                GraphServiceClient graphClient = await _graphClient.GetServiceClient();
                await graphClient.Groups[Id.ToString()]
                                .Request()
                                .DeleteAsync();
                return true;

            }
            catch (ServiceException)
            {
                return false;
            }
        }
    }
}
