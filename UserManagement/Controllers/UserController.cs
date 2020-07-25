using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using UserManagement.Services;
using UserManagement.Models;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/<UserController>
        [HttpGet]
        public async Task<OkObjectResult> Get()
        {
            users users = new users();
            try
            {
                users.Totalusers = new List<UserModel>();
                GraphServiceClient client = await GraphClient.GetServiceClient();
                var userList = await client.Users.Request().GetAsync();
                foreach (var user in userList)
                {
                    var objUser = DataHandler.UserProperty(user);
                    users.Totalusers.Add(objUser);
                }
                users.totalResults = users.Totalusers.Count;
                return Ok(users);
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    return Ok(ex.Message);
                }
                else
                {
                    return Ok(ex.Message);
                }
            }
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<OkObjectResult> Get(string id)
        {
            try
            {
                UserModel users = new UserModel();
                GraphServiceClient client = await GraphClient.GetServiceClient();
                var user = await client.Users[id].Request().GetAsync();
                users = DataHandler.UserProperty(user);
                return Ok(users);
            }
            catch(ServiceException ex)
            {
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    return Ok(ex.Message);
                }
                else
                {
                    return Ok(ex.Message);
                }

            }
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult<responseObject>> Post([FromBody] UserModel objUser)
        {
           GraphServiceClient client = await GraphClient.GetServiceClient();
           var user =  DataHandler.migrateToUserGraph(objUser);
            responseObject resObj = new responseObject();
            try
            {
                var response = await client.Users
                .Request()
                .AddAsync(user);

                objUser.Id = response.Id;
                resObj.data = objUser;
                resObj.status = true;
                return Ok(resObj);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
           
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
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
            catch (Exception ex)
            {
                return Ok(BadRequest());
            }
          
        }
    }
}
