using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Repository.Interface
{
    public interface IUserRepository
    {
        Task<ActionResult<users>> GetUsers();
        Task<ActionResult<UserModel>> GetUserById(Guid id);
        Task<Guid> AddUser([FromBody] UserModel objUser);
        Task<Guid> DeleteUserById(Guid id);
    }
}
