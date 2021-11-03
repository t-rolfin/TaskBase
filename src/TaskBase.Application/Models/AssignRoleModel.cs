using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Application.Models
{
    public record AssignRoleModel(string RoleName, string UserId);
    public record FireRoleModel(string RoleName, string UserId);
}
