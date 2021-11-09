using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Application.Models
{
    public record JwtToken(string access_token, string token_type);
}
