using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Models
{
    public class GroupModel
    {
        public string id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string MailNickname { get; set; }
        public bool? SecurityEnabled { get; set; }
        public List<Guid> Owners { get; set; }
        public List<Guid> Members { get; set; }
    }
    public class GetGroupModel
    {
        public string id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string MailNickname { get; set; }
        public bool? SecurityEnabled { get; set; }
        public DateTime createdDateTime { get; set; }
        public List<string> groupTypes { get; set; }
        public string mail { get; set; }
        public string visibility { get; set; }
    }
    public class GroupList
    {
        public int totalCount { get; set; }
        public List<GetGroupModel> Groups { get; set; }
    }

    public class GroupMember
    {
        public int totalCount { get; set; }
        public List<UserModel> GroupMembers { get; set; }
    }
}
