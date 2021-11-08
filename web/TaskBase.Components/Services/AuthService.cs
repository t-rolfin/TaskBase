using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OneOf;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;
using TaskBase.Components.Utils;
using TaskBase.Components.Views.Shared.Components.Avatar;

namespace TaskBase.Components.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _apiClient;
        private readonly IWebHostEnvironment _env;
        private readonly INotificationSender _notificationSender;

        public AuthService(
            IConfiguration configuration,
            HttpClient httpClient,
            IHttpContextAccessor contextAccessor,
            IWebHostEnvironment env,
            INotificationSender notificationSender)
        {
            _configuration = configuration
                ?? throw new ArgumentNullException(
                    "Can not provide an API base address because 'IConfiguration'is not initialized.");

            _apiClient = httpClient;
            _apiClient.BaseAddress = new Uri(_configuration["Api:BaseAddress"]);
            _contextAccessor = contextAccessor;
            _env = env;
            _notificationSender = notificationSender;
        }

        public async Task Register(RegistrationModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _apiClient.PostAsync("api/Account/Register", content);

            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            { }

        }

        public async Task<OneOf<JwtToken, FailApiResponse>> Login(LogInModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _apiClient.PostAsync("api/account/login", content);
            
            if(!response.IsSuccessStatusCode)
            {
                return await response.MapTo<FailApiResponse>();
            }

            var jwtToken = await response.MapTo<JwtToken>();
            await _contextAccessor.HttpContext.SignInWithJwtAsync(_configuration, jwtToken);

            return jwtToken;
        }

        public async Task LogOut()
        {
            await _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _contextAccessor.HttpContext.Session.Clear();
        }



        public async Task<List<UserModel>> GetMembersAsync()
        {
            var response = await _apiClient.GetAsync("api/Account/Users");
            return await response.MapTo<List<UserModel>>();
        }

        public async Task<List<string>> GetAvailableRolesAsync()
        {
            var response = await _apiClient.GetAsync("api/Account/Roles");
            return await response.MapTo<List<string>>();
        }

        public async Task<UserModel> AssignUserToRole(string roleName, string userId)
        {
            var model = new { roleName, userId };

            var content = new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json"
                );

            var response = await _apiClient.PutAsync("api/Account/Roles/Assign", content);

            _ = _notificationSender.GetResponse(response, out UserModel result)
                    .CreatePageNotification($"The '{roleName}' role was successfylly assigned to '{ result?.UserName }'.")
                    .SendAsync();

            return result ?? (await GetMembersAsync()).First(x => x.Id.Equals(userId));
        }

        public async Task<UserModel> FireUserToRole(string roleName, string userId)
        {
            var model = new { roleName, userId };

            var content = new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json"
                );

            var response = await _apiClient.PutAsync("api/Account/Roles/Fire", content);

            _ = _notificationSender.GetResponse(response, out UserModel userModel)
                    .CreatePageNotification($"The user '{userModel?.UserName}' was fired from the '{roleName}' role.")
                    .SendAsync();

            return userModel ?? (await GetMembersAsync()).First(x => x.Id.Equals(userId));
        }



        public async Task<AvatarModel> ChangeAvatar(byte[] avatarByteArray)
        {
            var form = new MultipartFormDataContent();
            form.Add(new ByteArrayContent(avatarByteArray, 0, avatarByteArray.Length), "avatar", "avatar.png");

            var response = await _apiClient.PutAsync("api/Avatar", form);

            _ = _notificationSender.GetResponse(response, out AvatarModel model)
                    .CreatePageNotification("Your avatar image was changed.")
                    .SendAsync();

            return model ?? new AvatarModel() { Url = "" };
        }

        public async Task<AvatarModel> GetAvatarUrlAsync(string userId)
        {
            var response = await _apiClient.GetAsync($"api/Avatar/{Guid.Parse(userId)}");
            return await response.MapTo<AvatarModel>();
        }

    }
}
