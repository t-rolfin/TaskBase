using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Components.Models
{
    public class UserModel
    {
        public UserModel(Guid userId, string userFullName)
        {
            UserId = userId;
            UserFullName = userFullName;
        }

        public Guid UserId { get; set; }
        public string UserFullName { get; set; }
    }
}
