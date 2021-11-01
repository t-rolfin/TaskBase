using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Data.Identity
{
    public class User : IdentityUser
    {
        public User() : base() { }

        public User(string userName, string email, string avatarUrl = "") 
            : this(userName, avatarUrl)
            => (Email) = (email);

        public User(string userName, string avatarUrl) : base(userName)
            => (AvatarUrl) = (avatarUrl);

        public string AvatarUrl { get; set; }
    }
}
