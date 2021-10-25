using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public record UserProfileModel(string Id, string UserName, string Email, string Token);
}
