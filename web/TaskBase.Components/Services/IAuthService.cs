using OneOf;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskBase.Components.Models;

namespace TaskBase.Components.Services
{
    public interface IAuthService
    {
        Task<OneOf<UserProfileModel, FailApiResponse>> Login(LogInModel model);
        Task LogOut();
        Task Register(RegistrationModel model);

        Task<List<UserModel>> GetMembersAsync();
        Task<List<string>> GetAvailableRolesAsync();
        Task<OneOf<UserModel, FailApiResponse>> AssignUserToRole(string roleName, string userId);
        Task<OneOf<UserModel, FailApiResponse>> FireUserToRole(string roleName, string userId);
    }
}