using API.Models;
using API.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Services.GeneralDataUsers
{
    public class GeneralDataUserRepository : IGeneralDataUserRepository
    {
        private readonly UserManager<GeneralDataUser> _userManager;
        private readonly SignInManager<GeneralDataUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GeneralDataUserRepository> _logger;

        public GeneralDataUserRepository(UserManager<GeneralDataUser> userManager, SignInManager<GeneralDataUser> signInManager, IConfiguration configuration, ILogger<GeneralDataUserRepository> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<GeneralDataUserDto> GetGeneralDataUser(string email)
        {
            _logger.LogWarning("Ejecutando la funcionalidad Obtener Datos Generales del Usuario");

            var user = await _userManager.FindByEmailAsync(email);

            if (user != null && user.EmailConfirmed)
            {
                var model = new GeneralDataUserDto
                {
                    Email = email,
                    Names = user.Names,
                    FirstLastName = user.FirstLastName,
                    SecondLastName = user.SecondLastName,
                    Title = user.Title,
                    MobileNumber = user.MobileNumber,
                    PhoneNumber = user.PhoneNumber,
                    Sex = user.Sex,
                    Address = user.Address,
                };

                return model;
            }

            return null;
        }

        public async Task<string> PostGeneralDataUser(GeneralDataUserDto model)
        {
            _logger.LogWarning("Ejecutando la funcionalidad Actualizar Datos Generales del Usuario");

            var user = await _userManager.FindByEmailAsync(model.Email);

            List<GeneralDataUser> users = _userManager.Users.ToList();

            /*if ((user.PhoneNumber == null && user.MobileNumber == null) && IsRepeatPhone(users, model.PhoneNumber, model.MobileNumber) > 0)
            {
                return "IsRepeated";
            }*/

            if ((user.PhoneNumber != null || user.MobileNumber != null) && IsRepeatPhone(users, model.PhoneNumber, model.MobileNumber) > 1)
            {
                return "IsRepeated";
            }

            if (user != null && user.EmailConfirmed)
            {
                user.Email = model.Email;
                user.Names = model.Names;
                user.FirstLastName = model.FirstLastName;
                user.SecondLastName = model.SecondLastName;
                user.Title = model.Title;
                user.MobileNumber = model.MobileNumber;
                user.PhoneNumber = model.PhoneNumber;
                user.Sex = model.Sex;
                user.Address = model.Address;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return "OkUserEdit";
                }
            }

            return "WrongUserEdit";
        }

        private int IsRepeatPhone(List<GeneralDataUser> users, string phoneNumber, string mobileNumber)
        {
            int cont = 0;

            if (users.Count() > 0)
            {                
                foreach (var user in users)
                {
                    if (user.PhoneNumber == phoneNumber || user.MobileNumber == mobileNumber)
                    {
                        cont++;
                    }
                }
            }
            return cont;

        }

        public async Task<bool> SuccessAuth(string email, string token, string isAuthenticated)
        {
            _logger.LogWarning("Ejecutando la validando la Autenticación del Usuario");

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
    }
}
