using API.Models;
using API.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace API.Repository.Services.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<GeneralDataUser> _userManager;
        private readonly SignInManager<GeneralDataUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<IdentityRepository> _logger;

        public UserRepository(UserManager<GeneralDataUser> userManager, SignInManager<GeneralDataUser> signInManager, IConfiguration configuration, ILogger<IdentityRepository> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<int> AddUser(UserDto userDto)
        {
            _logger.LogInformation("Ejecutando la funcionalidad Registro de Usuario");

            if (userDto.Email == "" || userDto.Password == "")
            {
                return -3;
            }

            if (await UserExist(userDto.Email))
            {
                return -1;
            }

            if (!userDto.Password.Equals(userDto.RepeatPassword))
            {
                return -2;
            }

            var userToCreate = new GeneralDataUser
            {
                UserName = userDto.Email,
                Email = userDto.Email
            };

            var result = await _userManager.CreateAsync(userToCreate, userDto.Password);

            if (result.Succeeded)
            {
                var userFromDb = await _userManager.FindByEmailAsync(userDto.Email);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(userFromDb);

                var uriBuilder = new UriBuilder(_configuration["ReturnPaths:ConfirmEmail"]);

                var query = HttpUtility.ParseQueryString(uriBuilder.Query);

                query["userid"] = userFromDb.Id;

                uriBuilder.Query = query.ToString();

                var urlString = uriBuilder.ToString();

                var senderEmail = _configuration["ReturnPaths:SenderEmail"];

                if (!await SendEmailAsync(senderEmail, userFromDb.Email, "Confirm you email address", urlString))
                {
                    return -501;
                }
            }

            var joinUserRole = await _userManager.AddToRoleAsync(userToCreate, "Customer");

            if (!joinUserRole.Succeeded)
            {
                return -4;
            }

            return 1;
        }

        public async Task<string> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return "UserNoExist";
            }
            else
            {
                //var result = await _userManager.DeleteAsync(user);

                user.IsActive = false;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return "Ok";
                }

                return "Error";
            }
        }

        public List<GeneralDataUser> GetUsers()
        {
            var users = _userManager.Users.ToList();

            return users;
        }

        public async Task<string> UpdateUser(UserUpdateDto userUpdateDto)
        {
            _logger.LogWarning("Ejecutando la funcionalidad Actualizar Usuario");

            var user = await _userManager.FindByEmailAsync(userUpdateDto.Email);

            List<GeneralDataUser> users = _userManager.Users.ToList();

            if ((user.PhoneNumber == "" && user.MobileNumber == "") && IsRepeatPhone(users, userUpdateDto.PhoneNumber, userUpdateDto.MobileNumber) > 0)
            {
                return "IsRepeated";
            }

            if ((user.PhoneNumber != "" && user.MobileNumber != "") && IsRepeatPhone(users, userUpdateDto.PhoneNumber, userUpdateDto.MobileNumber) > 1)
            {
                return "IsRepeated";
            }

            if (user != null && user.EmailConfirmed)
            {
                user.Email = userUpdateDto.Email;
                user.Names = userUpdateDto.Names;
                user.FirstLastName = userUpdateDto.FirstLastName;
                user.SecondLastName = userUpdateDto.SecondLastName;
                user.Title = userUpdateDto.Title;
                user.MobileNumber = userUpdateDto.MobileNumber;
                user.PhoneNumber = userUpdateDto.PhoneNumber;
                user.Sex = userUpdateDto.Sex;
                user.Address = userUpdateDto.Address;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return "OkUserEdit";
                }
            }

            return "WrongUserEdit";
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

        public async Task<GeneralDataUser> GetDataUser(string email)
        {
            _logger.LogWarning("Ejecutando la funcionalidad Obtener Datos Generales del Usuario");

            var user = await _userManager.FindByEmailAsync(email);

            if (user != null && user.EmailConfirmed)
            {
                var model = new GeneralDataUser
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

        public async Task<bool> UserExist(string email)
        {
            _logger.LogInformation("Ejecutando la funcionalidad Buscar de Usuario");

            var result = await _userManager.FindByEmailAsync(email);

            if (result != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> SendEmailAsync(string fromAddress, string toAddress, string subject, string message)
        {
            _logger.LogInformation("Ejecutando la funcionalidad Enviando Correo al Usuario");

            var mailMessage = new MailMessage(fromAddress, toAddress, subject, message);

            using (var client = new SmtpClient(_configuration["SMTP:Host"], int.Parse(_configuration["SMTP:Port"]))
            {
                Credentials = new NetworkCredential(_configuration["SMTP:Username"], _configuration["SMTP:Password"])
            })
            {
                await client.SendMailAsync(mailMessage);
                return true;
            }

        }
    }
}
