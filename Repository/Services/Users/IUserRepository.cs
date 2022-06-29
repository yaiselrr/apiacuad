using API.Models;
using API.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Repository.Services.Users
{
    public interface IUserRepository
    {
        Task<int> AddUser(UserDto userDto);
        Task<string> UpdateUser(UserUpdateDto userUpdateDto);
        Task<string> DeleteUser(string userId);
        List<GeneralDataUser> GetUsers();
        Task<GeneralDataUser> GetDataUser(string email);
        Task<bool> SuccessAuth(string email, string token, string isAuthenticated);
    }
}
