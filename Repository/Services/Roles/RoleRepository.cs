using API.Models;
using API.Models.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Services.Roles
{
    public class RoleRepository : IRoleRepository
    {
        private IMapper _mapper;
        private readonly ILogger<RoleRepository> _logger;
        private readonly UserManager<GeneralDataUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleRepository(IMapper mapper, ILogger<RoleRepository> logger, UserManager<GeneralDataUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<string> CreateRole(RoleDto roleDto)
        {
            _logger.LogInformation("Ejecutando la funcionalidad Crear Rol.");

            if (roleDto.RoleName == "")
            {
                return "NotEmpty";
            }

            var role = await _roleManager.FindByNameAsync(roleDto.RoleName);

            if (role != null)
            {
                return "IsRepeated";
            }

            var roleToCreate = new IdentityRole
            {
                Name = roleDto.RoleName
            };

            var result = await _roleManager.CreateAsync(roleToCreate);

            if (result.Succeeded)
            {
                return "Ok";
            }

            return "Error";
        }

        public async Task<string> DeleteRole(string id)
        {
            _logger.LogInformation("Ejecutando la funcionalidad Eliminar Rol por Id.");

            var role = await _roleManager.FindByIdAsync(id);

            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
                return "Ok";
            }

            return "NoRol";
        }

        public async Task<IdentityRole> GetRoleById(string id) 
        {
            _logger.LogInformation("Ejecutando la funcionalidad Obtener Rol por Id.");

            var role = await _roleManager.FindByIdAsync(id);

            if (role != null)
            {
                return role;
            }

            return null;
        }

        public async Task<List<IdentityRole>> GetRoles()
        {
            _logger.LogInformation("Ejecutando la funcionalidad Listar Roles");

            var result = await _roleManager.Roles.ToListAsync();

            if (result != null)
            {
                return result;
            }

            return null;
        }

        public async Task<string> UpdateRole(RoleUpdateDto roleUpdateDto)
        {
            _logger.LogInformation("Ejecutando la funcionalidad Actualizar Rol.");

            if (roleUpdateDto.RoleName == "" || roleUpdateDto.Id == "")
            {
                return "NotEmpty";
            }

            var role = await _roleManager.FindByIdAsync(roleUpdateDto.Id);

            if (role == null)
            {
                return "NoExist";
            }

            var roleToUpdate = new IdentityRole
            {
                Id = roleUpdateDto.Id,
                Name = roleUpdateDto.RoleName
            };

            role.Name = roleToUpdate.Name;

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return "Ok";
            }

            return "Error";
        }

        public async Task<bool> SuccessAuth(string email, string token, string isAuthenticated)
        {
            _logger.LogInformation("Ejecutando la validando la Autenticación del Usuario");

            string emailToken = "";

            if (email == "" || token == "" || isAuthenticated == "")
            {
                return false;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            if (jwt != null && isAuthenticated == "true")
            {
                emailToken = jwt.Claims.First(claim => claim.Type == "email").Value;
            }

            if (emailToken == null && !email.Equals(emailToken))
            {
                return false;
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null && !user.EmailConfirmed)
            {
                return false;
            }

            if (user != null && !user.EmailConfirmed)
            {
                return false;
            }

            return true;
        }

        public async Task<string> JoinUserRole(string idRole, string idUser)
        {
            var role = await _roleManager.FindByIdAsync(idRole);

            if (role == null)
            {
                return "NoRole";
            }

            var user = await _userManager.FindByIdAsync(idUser);

            if (user == null)
            {
                return "NoUser";
            }

            IdentityResult result = null;

            if (!await _userManager.IsInRoleAsync(user, role.Name))
            {
                result = await _userManager.AddToRoleAsync(user, role.Name);
            }

            if (result.Succeeded)
            {
                return "Ok";
            }

            return "Error";
            
        }
    }
}
