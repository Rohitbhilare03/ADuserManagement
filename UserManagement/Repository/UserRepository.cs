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
    public class UserRepository : IUserRepository
    {
        private readonly IDataHandler _dataHandler;
        private readonly IGraphClient _graphClient;

        public UserRepository(IDataHandler dataHandler, IGraphClient graphClient)
        {
            _dataHandler = dataHandler;
            _graphClient = graphClient;
        }

        public async Task<ActionResult<users>> GetUsers()
        {
            users users = new users();
            users.Totalusers = new List<UserModel>();
            GraphServiceClient client = await _graphClient.GetServiceClient();
            var userList = await client.Users.Request().GetAsync();
            foreach (var user in userList)
            {
                var objUser = _dataHandler.UserProperty(user);
                users.Totalusers.Add(objUser);
            }
            users.totalResults = users.Totalusers.Count;
            return users;
        }

        public async Task<ActionResult<UserModel>> GetUserById(Guid id)
        {
            try
            {
                UserModel user = new UserModel();
                GraphServiceClient client = await _graphClient.GetServiceClient();
                var singleUser = await client.Users[id.ToString()].Request().GetAsync();
                user = _dataHandler.UserProperty(singleUser);
                return user;
            }
            catch (ServiceException)
            {
                return null;
            }
           
        }

        public async Task<Guid> AddUser([FromBody] UserModel objUser)
        {
            GraphServiceClient client = await _graphClient.GetServiceClient();
            var user = _dataHandler.migrateToUserGraph(objUser);

            var UserAdded = await client.Users
            .Request()
            .AddAsync(user);

            objUser.Id = Guid.Parse(UserAdded.Id);

            return objUser.Id;

        }

        public async Task<Guid> DeleteUserById(Guid id)
        {
            
                GraphServiceClient client = await _graphClient.GetServiceClient();
                await client.Users[id.ToString()]
                    .Request()
                    .DeleteAsync();
                return id;
            
            
            

        }
    }
}
