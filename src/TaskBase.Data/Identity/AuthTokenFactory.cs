using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using TaskBase.Application.Services;
using Microsoft.AspNetCore.Identity;

namespace TaskBase.Data.Identity
{
    public class AuthTokenFactory : IAuthTokenFactory
    {
        readonly IConfiguration _configuration;
        readonly ILoginService<User> _loginService;

        public AuthTokenFactory(IConfiguration configuration,
            ILoginService<User> loginService)
        {
            _configuration = configuration;
            _loginService = loginService;
        }

        public async Task<string> GetToken(Guid UserId)
        {
            var tokenHeader = new JwtHeader(
                new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                        SecurityAlgorithms.HmacSha256
                    ));

            var user = await _loginService.FindUserByIdAsync(UserId.ToString());

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString())
            };

            foreach (var role in (await _loginService.GetRolesAsync(user)).ToList())
                claims.Add(new Claim(ClaimTypes.Role, role));

            var tokenPayload = new JwtPayload(claims);
            var tokenBuilder = new JwtSecurityToken(tokenHeader, tokenPayload);

            return new JwtSecurityTokenHandler().WriteToken(tokenBuilder);
        }

    }
}
