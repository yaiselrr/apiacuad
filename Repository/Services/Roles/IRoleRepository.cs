using API.Models.Dto;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Repository.Services.Roles
{
    public interface IRoleRepository
    {
        Task<List<IdentityRole>> GetRoles();

        Task<IdentityRole> GetRoleById(string id);

        Task<string> CreateRole(RoleDto roleDto);

        Task<string> UpdateRole(RoleUpdateDto roleUpdateDto);

        Task<string> DeleteRole(string id);

        Task<bool> SuccessAuth(string email, string token, string isAuthenticated);

        Task<string> JoinUserRole(string idRole, string idUser);
    }
}
