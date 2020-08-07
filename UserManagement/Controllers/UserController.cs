using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using UserManagement.Models;
using System.Net;
using UserManagement.Repository.Interface;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _UserRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            _UserRepository = userRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            try
            {
                var users = await _UserRepository.GetUsers();
                return Ok(users);
            }
            catch (ServiceException)
            {
                return BadRequest();
            }
        }

        [HttpGet("id", Name = "GetUser")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UserModel>> GetUserById(Guid id)
        {

            var user = await _UserRepository.GetUserById(id);
            if (user == null)
            {
                _logger.LogError($"User with id : {id} not found");
                return NotFound();
            }
            return Ok(user);
        }
    
    [HttpPost]
    [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<UserModel>> AddUser([FromBody] UserModel objUser)
    {
        try
        {
            var createdId = await _UserRepository.AddUser(objUser);
            return CreatedAtRoute("GetUser", new { id = createdId }, objUser);
        }
        catch (Exception)
        {
            return BadRequest();
        }

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Guid>> DeleteUserById(Guid id)
    {
        try
        {
            var deletedId = await _UserRepository.DeleteUserById(id);
            return Ok(deletedId);
        }
        catch (ServiceException)
        {
            return NotFound();
        }

    }
}
}
