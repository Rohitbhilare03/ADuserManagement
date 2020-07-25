using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services
{
    public class DataHandler
    {
        public static UserModel UserProperty(Microsoft.Graph.User graphUser)
        {
            UserModel user = new UserModel();
            user.Id = graphUser.Id;
            user.AccountEnabled = graphUser.AccountEnabled;
            user.DisplayName = graphUser.DisplayName;
            user.MailNickname = graphUser.MailNickname;
            user.GivenName = graphUser.GivenName;
            user.Surname = graphUser.Surname;
            user.UserPrincipalName = graphUser.UserPrincipalName;
            user.Email = graphUser.Mail;

            return user;
        }

        public static User migrateToUserGraph(UserModel objUser)
        {
            User userGraph = new User();
            userGraph.AccountEnabled = objUser.AccountEnabled;
            userGraph.DisplayName = objUser.DisplayName;
            userGraph.MailNickname = objUser.MailNickname;
            userGraph.PasswordProfile = new PasswordProfile();
            userGraph.PasswordProfile.Password = objUser.PasswordProfile.Password;
            userGraph.PasswordProfile.ForceChangePasswordNextSignIn = objUser.PasswordProfile.ForceChangePasswordNextSignIn;
            userGraph.UserPrincipalName = objUser.UserPrincipalName;
            userGraph.GivenName = objUser.GivenName;
            userGraph.PasswordPolicies = objUser.PasswordPolicies;
            userGraph.Surname = objUser.Surname;
            return userGraph;
        }

    }
}
