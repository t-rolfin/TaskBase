using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Components.Models
{
    public record UserModel(string Id, string UserName, List<string> AvailableRoles);
}
