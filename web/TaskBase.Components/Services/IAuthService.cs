using OneOf;
using System.Threading.Tasks;
using TaskBase.Components.Models;

namespace TaskBase.Components.Services
{
    public interface IAuthService
    {
        Task<OneOf<UserProfileModel, FailApiResponse>> Login(LogInModel model);
        Task LogOut();
        Task Register(RegistrationModel model);
    }
}