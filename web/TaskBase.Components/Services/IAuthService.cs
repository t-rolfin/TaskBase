using OneOf;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskBase.Components.Models;
using TaskBase.Components.Views.Shared.Components.Avatar;

namespace TaskBase.Components.Services
{
    public interface IAuthService
    {
        Task<OneOf<JwtToken, FailApiResponse>> Login(LogInModel model);
        Task LogOut();
        Task Register(RegistrationModel model);

        Task<List<UserModel>> GetMembersAsync();
        Task<List<string>> GetAvailableRolesAsync();
        Task<UserModel> AssignUserToRole(string roleName, string userId);
        Task<UserModel> FireUserToRole(string roleName, string userId);

        Task<AvatarModel> ChangeAvatar(byte[] avatarByteArray);
        Task<AvatarModel> GetAvatarUrlAsync(string userId);
    }
}