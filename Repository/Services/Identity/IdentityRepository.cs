using API.Models;
using API.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace API.Repository
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly UserManager<GeneralDataUser> _userManager;
        private readonly SignInManager<GeneralDataUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<IdentityRepository> logger;
        private readonly string registerNotificationHtmlBody = @"<table style=""max-width: 600px;padding: 10px;margin: 0 auto;border-collapse: collapse;"">
		
		
		<tr class=""mb-4"">
			<td>
				<div style=""color: #34495e;margin: 4% 10% 2%; text-align: justify;font-family: sans-serif;"">
					<h2 style=""color: #0099a8;margin: 0 0 7px;"">Estimada/o usuaria/o</h2>
					<p style=""margin: 2px; font-size: 15px;"">
						Se registró el alta de usuario para el servicio SIMAS, para poder continuar, por favor dar click en el siguiente enlace:
					</p>
					<p style =""margin: 2px; font-size: 15px;"">
                        Confirmación de registro
                    </p>
					<p style = ""margin: 2px; font-size: 15px;"">
                        @UrlLink
                    </p>
                    <p style=""margin: 2px; font-size: 15px;"">
                        Este enlace estará disponible las siguiente[n horas],
					</p>
                    <p style=""margin: 2px; font-size: 15px;"">
                        una vez pasado este tiempo, tendrás que registrar de nuevo el usuario en:
					</p>
					<p style =""margin: 2px; font-size: 15px;"">
                        @Register
                    </p>
                </div>		
			</td>
		</tr>
        <tr>
			<td style=""text-align: left;padding: 0;"">
				<img src = ""09.png"" width=""20%"" style=""display: block;margin: 1.5% 3%;"">	
				<p style = ""margin: 2px; font-size: 15px;"" >
                        Para más información favor de contactarnos al 871 74 91 700
				</p>			
			</td>
		</tr>
		
		
	</table>";
        private readonly string restorePasswordNotificationHtmlBody = @"<table style=""max-width: 600px;padding: 10px;margin: 0 auto;border-collapse: collapse;"">
		
		
		<tr class=""mb-4"">
			<td>
				<div style=""color: #34495e;margin: 4% 10% 2%; text-align: justify;font-family: sans-serif;"">
					<h2 style=""color: #0099a8;margin: 0 0 7px;"">Estimada/o usuaria/o @Name</h2>
					<p style=""margin: 2px; font-size: 15px;"">
						Se registró la solicitud de cambio de contraseña, a la cual podrás acceder en el siguiente enlace:
					</p>
					<p style = ""margin: 2px; font-size: 15px;"">
                        @UrlLinkRestorePassword
                    </p>
                    <p style=""margin: 2px; font-size: 15px;"">
                        Si no solicitaste el cambio de contraseña, favor de hacer caso omiso de este mensaje
					</p>
                </div>		
			</td>
		</tr>
        <tr>
			<td style=""text-align: left;padding: 0;"">
				<img src = ""09.png"" width=""20%"" style=""display: block;margin: 1.5% 3%;"">	
				<p style = ""margin: 2px; font-size: 15px;"" >
                        Para más información favor de contactarnos al 871 74 91 700
				</p>			
			</td>
		</tr>
		
		
	</table>";

        public IdentityRepository(UserManager<GeneralDataUser> userManager, SignInManager<GeneralDataUser> signInManager, IConfiguration configuration, ILogger<IdentityRepository> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            this.logger = logger;
        }
        public async Task<string> ConfirmEmail(ConfirmEmail model)
        {
            logger.LogInformation("Ejecutando la funcionalidad Confirmacion del Registro de Usuario");

            if (string.IsNullOrEmpty(model.UserId))
            {
                return "NotEmpty";
            }

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user.EmailConfirmed)
            {
                return "IsConfirmed";
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                return "WrongConfirm";
            }
            else
            {
                return "ConfirmOk";
            }
        }

        public async Task<string> Login(UserLoginDto userLoginDto)
        {
            logger.LogInformation("Ejecutando la funcionalidad Autenticación de Usuario");

            if (string.IsNullOrEmpty(userLoginDto.Email) || string.IsNullOrEmpty(userLoginDto.Password))
            {
                return "NotEmpty";
            }

            var user = await _userManager.FindByEmailAsync(userLoginDto.Email);

            if (user == null)
            {
                return "NoUser";
            }
            
            if (user.IsActive == false)
            {
                return "Disabled";
            }

            if (!user.EmailConfirmed)
            {
                return "NoConfirm";
            }

            var result = await _signInManager.PasswordSignInAsync(userLoginDto.Email, userLoginDto.Password, userLoginDto.Remember, false);

            if (!result.Succeeded)
            {
                return "WrongPassword";
            }
            else
            {
                return GenerateToken(user);
            }
        }

        public async Task<int> Register(string email, string password, string confirmPassword)
        {
            logger.LogInformation("Ejecutando la funcionalidad Registro de Usuario");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return -3;
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                return -1;
            }

            if (!password.Equals(confirmPassword))
            {
                return -2;
            }

            var userToCreate = new GeneralDataUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(userToCreate, password);

            if (result.Succeeded)
            {
                var userFromDb = await _userManager.FindByEmailAsync(email);

                //var url = signatureNotificationHtmlBody.Replace("@UrlLink", userId.ToString()).Replace("@id", SecurityId);
                //var htmlUrl = htmlText.Replace("@UrlLink", url).Replace("@UserName", username);

                //var token = await _userManager.GenerateEmailConfirmationTokenAsync(userFromDb);

                var unRealToken = "CfDJ8NS42a7j+69Lj8N3z0WunckgFH8HsHJ6+yvcZhYONPrpnn1SWXsfPwcOcjAsNdFEBJ+FXEP3Hd7cayuaUL0qv7zL4+JhtDwNKeiVCQYzSCkcZKYcOx7ZTyJK9ExmTcP5rIw9wsh8LcpWnQLRKRMDAmI2ztKQEdSeFJH0vTvKs2p99sPuAR5GQcRduae2PCCqMH7RNknbQJzXQ+csVrR/";

                var uriBuilder = new UriBuilder(_configuration["ReturnPaths:ConfirmEmail"]) + unRealToken + userFromDb.Id;

                var htmlUrl = registerNotificationHtmlBody.Replace("@UrlLink", uriBuilder.ToString()).Replace("@Register", new UriBuilder(_configuration["ReturnPaths:Register"]).ToString());
                var htmlUrlEnd = htmlUrl.Replace("@UrlLinkRegister", _configuration["ReturnPaths:Register"]);

                var urlString = htmlUrlEnd;

                var senderEmail = _configuration["ReturnPaths:SenderEmail"];

                if (!await SendEmailAsync(senderEmail, userFromDb.Email, "Correo de confirmación del registro de usuario", urlString))
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

        public async Task<bool> UserExist(string email)
        {
            logger.LogInformation("Ejecutando la funcionalidad Buscar de Usuario");

            var result = await _userManager.FindByEmailAsync(email);

            if (result != null)
            {
                return true;
            }
            return false;
        }

        private string GenerateToken(GeneralDataUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _configuration["Token:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> SendEmailAsync(string fromAddress, string toAddress, string subject, string message)
        {
            logger.LogInformation("Ejecutando la funcionalidad Enviando Correo al Usuario");

            MailMessage mailMessage = new MailMessage(fromAddress, toAddress, subject, message);

            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;

            using (var client = new SmtpClient(_configuration["SMTP:Host"], int.Parse(_configuration["SMTP:Port"]))
            {
                Credentials = new NetworkCredential(_configuration["SMTP:Username"], _configuration["SMTP:Password"])
            })
            {
                await client.SendMailAsync(mailMessage);
                return true;
            }

        }
        public async Task<bool> SendEmailAsync2(string fromAddress, string toAddress, string subject, string message)
        {
            logger.LogInformation("Ejecutando la funcionalidad Enviando Correo al Usuario");

            MailMessage msg = new MailMessage();
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            try
            {
                msg.Subject = subject;
                msg.Body = message;
                msg.From = new MailAddress(fromAddress);
                msg.To.Add(toAddress);
                msg.IsBodyHtml = true;
                client.Host = _configuration["SMTP:Host"];
                System.Net.NetworkCredential basicauthenticationinfo = new System.Net.NetworkCredential(_configuration["SMTP:Username"], _configuration["SMTP:Password"]);
                client.Port = int.Parse(_configuration["SMTP:Port"]);
                //client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicauthenticationinfo;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
                //log.Error(ex.Message);
            }

            /*var mailMessage = new MailMessage(fromAddress, toAddress, subject, message);

            using (var client = new SmtpClient(_configuration["SMTP:Host"], int.Parse(_configuration["SMTP:Port"]))
            {
                Credentials = new NetworkCredential(_configuration["SMTP:Username"], _configuration["SMTP:Password"])
            })
            {
                await client.SendMailAsync(mailMessage);
                return true;
            }*/

        }

        public async Task<string> ResetPassword(string id, string password, string confirmPassword)
        {
            logger.LogInformation("Ejecutando la funcionalidad Resetear Contraseña de Usuario");

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password))
            {
                return "NotEmpty";
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return "NoExist";
            }

            if (!password.Equals(confirmPassword))
            {
                return "NoEquals";
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, password);

            if (!result.Succeeded)
            {
                return "WrongResetPassword";
            }
            else
            {
                return "PasswordReseted";
            }
        }

        public async Task<int> ForgetPassword(string email)
        {
            logger.LogInformation("Ejecutando la funcionalidad Recuperar Contraseña de Usuario");

            if (string.IsNullOrEmpty(email))
            {
                return -2;
            }
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return -1;
            }

            else
            {

                //var uriBuilder = new UriBuilder(_configuration["ReturnPaths:ResetPassword"]);
                var unRealToken = "CfDJ8NS42a7j+69Lj8N3z0WunckgFH8HsHJ6+yvcZhYONPrpnn1SWXsfPwcOcjAsNdFEBJ+FXEP3Hd7cayuaUL0qv7zL4+JhtDwNKeiVCQYzSCkcZKYcOx7ZTyJK9ExmTcP5rIw9wsh8LcpWnQLRKRMDAmI2ztKQEdSeFJH0vTvKs2p99sPuAR5GQcRduae2PCCqMH7RNknbQJzXQ+csVrR/";

                var uriBuilder = new UriBuilder(_configuration["ReturnPaths:ResetPassword"]) + unRealToken + user.Id;

                //var query = HttpUtility.ParseQueryString(uriBuilder.Query);

                //query["userId"] = user.Id;

                //uriBuilder.Query = query.ToString();

                //var urlString = uriBuilder.ToString();

                var htmlUrl = restorePasswordNotificationHtmlBody.Replace("@UrlLink", uriBuilder.ToString()).Replace("@Name", user.Names);
                //var htmlUrlEnd = htmlUrl.Replace("@UrlLinkRegister", _configuration["ReturnPaths:Register"]);

                var urlString = htmlUrl;

                var senderEmail = _configuration["ReturnPaths:SenderEmail"];

                if (!await SendEmailAsync(senderEmail, user.Email, "Correo de recuperación de contraseña", urlString))
                {
                    return -501;
                }

            }

            return 1;
        }

        public async Task<string> ChangePassword(PasswordChangeDto model)
        {
            logger.LogInformation("Ejecutando la funcionalidad Cambiar Contraseña de Usuario");

            if (string.IsNullOrEmpty(model.Email))
            {
                return "NotExistEmail";
            }

            if (string.IsNullOrEmpty(model.OldPassword) || string.IsNullOrEmpty(model.NewPassword) || string.IsNullOrEmpty(model.ConfirmPassword))
            {
                return "FieldNull";
            }

            if (await UserExist(model.Email) == false)
            {
                return "NotExistUser";
            }

            if (!model.NewPassword.Equals(model.ConfirmPassword))
            {
                return "DiferentPassword";
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return "ErrorChangePassword";
            }

            await _signInManager.RefreshSignInAsync(user);

            return "Ok";
        }

        public async Task<bool> SuccessAuth(string email, string token, string isAuthenticated)
        {
            logger.LogInformation("Ejecutando la validando la Autenticación del Usuario");

            string emailToken = "";

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(isAuthenticated))
            {
                return false;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            if (jwt != null && isAuthenticated == "true")
            {
                emailToken = jwt.Claims.First(claim => claim.Type == "email").Value;
            }

            if (string.IsNullOrEmpty(emailToken) && !email.Equals(emailToken))
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

        public async Task<string> Logout()
        {
            await _signInManager.SignOutAsync();
            return "Closed";

        }

        public async Task<bool> DeleteUser(string email)
        {
            logger.LogInformation("Ejecutando la funcionalidad Buscar de Usuario");

            var result = await _userManager.FindByEmailAsync(email);

            if (result != null)
            {
                var response = await _userManager.DeleteAsync(result);
                if (response.Succeeded)
                {
                    return true;
                }
                
            }
            return false;
        }
    }
}
