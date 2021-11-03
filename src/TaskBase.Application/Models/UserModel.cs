using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Application.Models
{
    public class UserModel
    {
        public UserModel(Guid id, string userName, string avatarUrl, List<string> userRoles)
        {
            Id = id;
            UserName = userName;
            AvatarUrl = avatarUrl;
            UserRoles = userRoles;
        }

        public UserModel(Guid id, string userName, string avatarUrl)
            : this(id, userName, avatarUrl, new List<string>()) { }

        public UserModel() => (UserRoles) = (new List<string>());

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }

        public List<string> UserRoles { get; set; }
    }
}
