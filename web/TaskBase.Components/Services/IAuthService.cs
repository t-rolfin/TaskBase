using System.Threading.Tasks;
using TaskBase.Components.Models;

namespace TaskBase.Components.Services
{
    public interface IAuthService
    {
        Task<UserProfileModel> Login(LogInModel model);
        Task LogOut();
        Task Register(RegistrationModel model);
    }
}