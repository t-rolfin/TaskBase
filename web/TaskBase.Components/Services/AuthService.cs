using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;

namespace TaskBase.Components.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _apiClient;

        public AuthService(IConfiguration configuration,
            HttpClient httpClient, IHttpContextAccessor contextAccessor)
        {
            _configuration = configuration
                ?? throw new ArgumentNullException(
                    "Can not provide an API base address because 'IConfiguration'is not initialized.");

            _apiClient = httpClient;
            _apiClient.BaseAddress = new Uri(_configuration["Api:BaseAddress"]);
            _contextAccessor = contextAccessor;
        }

        public async Task Register(RegistrationModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _apiClient.PostAsync("api/Account/Register", content);

            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            { }

        }

        public async Task<UserProfileModel> Login(LogInModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _apiClient.PostAsync("api/account/login", content);
            response.EnsureSuccessStatusCode();

            var profile = await GenerateResponse<UserProfileModel>(response.Content);

            if (response.IsSuccessStatusCode)
            {

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

                tokenHandler.ValidateToken(profile.access_token, new TokenValidationParameters
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

                await _contextAccessor.HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme, 
                        principal
                    );

                _contextAccessor.HttpContext.Session.SetString("access_token", profile.access_token);
            }

            return profile;
        }

        public async Task LogOut()
        {
            await _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _contextAccessor.HttpContext.Session.Clear();
        }

        async Task<T> GenerateResponse<T>(HttpContent content)
        {
            string Content = await content.ReadAsStringAsync();
            var stringContent = JsonConvert.DeserializeObject<T>(Content);

            return stringContent;
        }

    }
}
