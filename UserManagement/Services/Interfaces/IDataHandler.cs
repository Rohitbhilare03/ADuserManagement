using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Interfaces
{
    public interface IDataHandler
    {
        UserModel UserProperty(User graphUser);
        User migrateToUserGraph(UserModel objUser);
    }
}
