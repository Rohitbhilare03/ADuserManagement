using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Models
{

    public class users
    {
        public int totalResults { get; set; }
        public List<UserModel> Totalusers { get; set; }
    }
    public class UserModel
    {
        public string Id { get; set; }
        public bool? AccountEnabled { get; set; }
        public string DisplayName { get; set; }

        public string MailNickname { get; set; }
        public string UserPrincipalName { get; set; }
        public string GivenName { get; set; }
        public string PasswordPolicies { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public passwordProfile PasswordProfile { get; set; }

    }

    public class passwordProfile
    {
        public string Password { get; set; }
        public bool ForceChangePasswordNextSignIn { get; set; }
    }

}