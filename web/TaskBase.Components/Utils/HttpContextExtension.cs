using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;

namespace TaskBase.Components.Utils
{
    public static class HttpContextExtension
    {
        public static async Task SignInWithJwtAsync(this HttpContext context, IConfiguration configuration, JwtToken token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

            tokenHandler.ValidateToken(token.access_token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;


            var claimsIdentity = new ClaimsIdentity(jwtToken.Claims, "My Identity");

            var principal = new ClaimsPrincipal(claimsIdentity);

            await context.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal
                );

            context.Session.SetString("access_token", token.access_token);
        }
    }
}
