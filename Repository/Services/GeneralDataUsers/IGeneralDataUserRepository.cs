using API.Models;
using API.Models.Dto;
using System.Threading.Tasks;

namespace API.Repository.Services.GeneralDataUsers
{
    public interface IGeneralDataUserRepository
    {
        Task<GeneralDataUserDto> GetGeneralDataUser(string email);

        Task<string> PostGeneralDataUser(GeneralDataUserDto model);

        Task<bool> SuccessAuth(string email, string token, string isAuthenticated);
    }
}
