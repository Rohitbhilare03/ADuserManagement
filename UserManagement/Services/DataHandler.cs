using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Interfaces;

namespace UserManagement.Services
{
    public class DataHandler : IDataHandler
    {
        public UserModel UserProperty(User graphUser)
        {
            UserModel user = new UserModel();
            user.Id = Guid.Parse(graphUser.Id);
            user.AccountEnabled = graphUser.AccountEnabled;
            user.DisplayName = graphUser.DisplayName;
            user.MailNickname = graphUser.MailNickname;
            user.GivenName = graphUser.GivenName;
            user.Surname = graphUser.Surname;
            user.UserPrincipalName = graphUser.UserPrincipalName;
            user.Email = graphUser.Mail;
            return user;
        }

        public User migrateToUserGraph(UserModel objUser)
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

        public GetGroupModel GroupBind(Group group)
        {
            GetGroupModel groupModel = new GetGroupModel();
            groupModel.id = group.Id;
            groupModel.DisplayName = group.DisplayName;
            groupModel.Description = group.Description;
            groupModel.MailNickname = group.MailNickname;
            groupModel.groupTypes = group.GroupTypes.ToList();
            groupModel.mail = group.Mail;
            groupModel.visibility = group.Visibility;
            groupModel.createdDateTime = (group.CreatedDateTime).Value.UtcDateTime;

            groupModel.SecurityEnabled = group.SecurityEnabled;

            return groupModel;
        }


        public async Task<ActionResult<Group>> AssignDataToAddGroup(GroupModel objGroup)
        {
            Group GroupObject = new Group();
            if (objGroup.Owners.Count > 0 || objGroup.Members.Count > 0)
            {
                var additionalData = new Dictionary<string, object>()
                {
                    {"owners@odata.bind", new List<string>()},
                    {"members@odata.bind", new List<string>()}
                };
                if (objGroup.Owners.Count > 0)
                {
                    foreach (Guid item in objGroup.Owners)
                    {
                        (additionalData["owners@odata.bind"] as List<string>).Add("https://graph.microsoft.com/v1.0/users/" + item);
                    }
                }
                if (objGroup.Members.Count > 0)
                {
                    foreach (Guid item in objGroup.Members)
                    {
                        (additionalData["members@odata.bind"] as List<string>).Add("https://graph.microsoft.com/v1.0/users/" + item);
                    }
                }

                GroupObject.AdditionalData = additionalData;
            }

            List<string> groupType = new List<string>();
            groupType.Add("Unified");

            GroupObject.GroupTypes = groupType;
            GroupObject.SecurityEnabled = objGroup.SecurityEnabled;
            GroupObject.MailNickname = objGroup.MailNickname;
            GroupObject.DisplayName = objGroup.DisplayName;
            GroupObject.MailEnabled = true;
            GroupObject.Description = objGroup.Description;
            return GroupObject;
        }
    }
}
