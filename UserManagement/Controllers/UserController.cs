using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using UserManagement.Services;
using UserManagement.Models;
using System.Net;
using UserManagement.Services.Interfaces;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IDataHandler _dataHandler;
        private readonly ILogger<UserController> _logger;

        public UserController(IDataHandler dataHandler, ILogger<UserController> logger)
        {
            _dataHandler = dataHandler;
            _logger = logger;
        }

       

      
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<UserModel>>> Get()
        {
            users users = new users();
            try
            {
                users.Totalusers = new List<UserModel>();
                GraphServiceClient client = await GraphClient.GetServiceClient();
                var userList = await client.Users.Request().GetAsync();
                foreach (var user in userList)
                {
                    var objUser = _dataHandler.UserProperty(user);
                    users.Totalusers.Add(objUser);
                }
                users.totalResults = users.Totalusers.Count;
                return Ok(users);
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(ex.Message);
                }
            }
        }

        [HttpGet("{id:length(36)}", Name = "GetUser")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UserModel>> Get(string id)
        {
            try
            {
                UserModel users = new UserModel();
                GraphServiceClient client = await GraphClient.GetServiceClient();
                var user = await client.Users[id].Request().GetAsync();
                users = _dataHandler.UserProperty(user);
                return Ok(users);
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogError($"User with id : {id} not found");
                    return NotFound();
                }
                else
                {
                    return Ok(ex.Message);
                }

            }
        }
        [HttpPost]
        [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UserModel>> Post([FromBody] UserModel objUser)
        {
            GraphServiceClient client = await GraphClient.GetServiceClient();
            var user = _dataHandler.migrateToUserGraph(objUser);
            try
            {
                var UserAdded = await client.Users
                .Request()
                .AddAsync(user);

                objUser.Id = UserAdded.Id;

                return CreatedAtRoute("GetUser", new { id = UserAdded.Id }, objUser);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpDelete("{id:length(36)}")]
        public async Task<OkObjectResult> Delete(string id)
        {
            try
            {
                GraphServiceClient client = await GraphClient.GetServiceClient();
                await client.Users[id]
                    .Request()
                    .DeleteAsync();
                return Ok(id);
            }
            catch (Exception)
            {
                return Ok(BadRequest());
            }

        }
    }
}
