using API.Models;
using API.Models.Dto;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace API.Repository
{
    public interface IIdentityRepository
    {
        Task<int> Register(string email, string password, string confirmPassword);
        Task<string> Login(UserLoginDto userLoginDto);
        Task<string> ConfirmEmail(ConfirmEmail model);
        Task<string> ResetPassword(string id, string password, string confirmPassword);
        Task<bool> UserExist(string email);
        Task<int> ForgetPassword(string email);
        Task<string> ChangePassword(PasswordChangeDto model);
        Task<bool> SuccessAuth(string email, string token, string isAuthenticated);
        Task<string> Logout();
        Task<bool> DeleteUser(string email);
    }
}
