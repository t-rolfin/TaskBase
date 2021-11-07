﻿using Microsoft.AspNetCore.Authentication;
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
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;
using TaskBase.Components.Views.Shared.Components.Avatar;

namespace TaskBase.Components.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _apiClient;
        private readonly IWebHostEnvironment _env;

        public AuthService(IConfiguration configuration,
            HttpClient httpClient, IHttpContextAccessor contextAccessor, IWebHostEnvironment env)
        {
            _configuration = configuration
                ?? throw new ArgumentNullException(
                    "Can not provide an API base address because 'IConfiguration'is not initialized.");

            _apiClient = httpClient;
            _apiClient.BaseAddress = new Uri(_configuration["Api:BaseAddress"]);
            _contextAccessor = contextAccessor;
            _env = env;
        }

        public async Task Register(RegistrationModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _apiClient.PostAsync("api/Account/Register", content);

            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            { }

        }

        public async Task<OneOf<UserProfileModel, FailApiResponse>> Login(LogInModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _apiClient.PostAsync("api/account/login", content);
            
            if(!response.IsSuccessStatusCode)
            {
                return await GenerateResponse<FailApiResponse>(response.Content);
            }

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



        public async Task<List<UserModel>> GetMembersAsync()
        {
            var response = await _apiClient.GetAsync("api/Account/Users");
            return await GenerateResponse<List<UserModel>>(response.Content);
        }

        public async Task<List<string>> GetAvailableRolesAsync()
        {
            var response = await _apiClient.GetAsync("api/Account/Roles");
            return await GenerateResponse<List<string>>(response.Content);
        }

        public async Task<OneOf<UserModel, FailApiResponse>> AssignUserToRole(string roleName, string userId)
        {
            var model = new { roleName, userId };

            var content = new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json"
                );

            var response = await _apiClient.PutAsync("api/Account/Roles/Assign", content);

            return response.IsSuccessStatusCode
                ? await GenerateResponse<UserModel>(response.Content)
                : await GenerateResponse<FailApiResponse>(response.Content);
        }

        public async Task<OneOf<UserModel, FailApiResponse>> FireUserToRole(string roleName, string userId)
        {
            var model = new { roleName, userId };

            var content = new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json"
                );

            var response = await _apiClient.PutAsync("api/Account/Roles/Fire", content);

            return response.IsSuccessStatusCode
                ? await GenerateResponse<UserModel>(response.Content)
                : await GenerateResponse<FailApiResponse>(response.Content);
        }


        public async Task<OneOf<AvatarModel, FailApiResponse>> ChangeAvatar(byte[] avatarByteArray)
        {
            var form = new MultipartFormDataContent();
            form.Add(new ByteArrayContent(avatarByteArray, 0, avatarByteArray.Length), "avatar", "avatar.png");
            form.Add(new StringContent(_env.WebRootPath), "baseDirectory");

            var response = await _apiClient.PutAsync("api/Avatar", form);

            return await GenerateResponse<AvatarModel>(response.Content);
        }

        public async Task<AvatarModel> GetAvatarUrlAsync(string userId)
        {
            var response = await _apiClient.GetAsync($"api/Avatar/{Guid.Parse(userId)}");
            return await GenerateResponse<AvatarModel>(response.Content);
        }

        async Task<T> GenerateResponse<T>(HttpContent content)
        {
            string Content = await content.ReadAsStringAsync();
            var stringContent = JsonConvert.DeserializeObject<T>(Content);

            return stringContent;
        }
    }
}
