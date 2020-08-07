using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Repository.Interface;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityGroupController : ControllerBase
    {
        private readonly IGroupRepository _groupRepository;
        private readonly ILogger<SecurityGroupController> _logger;

        public SecurityGroupController(IGroupRepository groupRepository, ILogger<SecurityGroupController> logger)
        {
            _groupRepository = groupRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetGroupModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<GetGroupModel>> GetAllGroups()
        {
            try
            {
                var groups = await _groupRepository.GetAllGroups();
                return Ok(groups);
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }
        }

        [HttpGet("id", Name = "GetGroupById")]
        [ProducesResponseType(typeof(GetGroupModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<GetGroupModel>> GetGroupById(Guid Id)
        {
            try
            {
                var groups = await _groupRepository.GetGroupById(Id);
                return Ok(groups);
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(GroupModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<GroupModel>> CreateGroup([FromBody] GroupModel objGroup)
        {
            try
            {
                var createdId = await _groupRepository.CreateGroup(objGroup);
                return CreatedAtRoute("GetGroupById", new { id = createdId }, objGroup);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpGet("Member/id", Name = "GetGroupMembers")]
        [ProducesResponseType(typeof(GetGroupModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<GetGroupModel>> GetGroupMemberById(Guid Id)
        {
            try
            {
                var Member = await _groupRepository.GetGroupMembersByGroupId(Id);
                return Ok(Member);
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }
        }

        [HttpGet("Owner/id", Name = "GetGroupOwner")]
        [ProducesResponseType(typeof(GetGroupModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<GetGroupModel>> GetGroupOwnerById(Guid Id)
        {
            try
            {
                var Owner = await _groupRepository.GetGroupMembersByGroupId(Id);
                return Ok(Owner);
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Guid>> DeleteUserById(Guid id)
        {
            try
            {
                var deletedId = await _groupRepository.DeleteGroup(id);
                return Ok(deletedId);
            }
            catch (ServiceException)
            {
                return NotFound();
            }

        }
    }
}
