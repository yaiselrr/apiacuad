using API.Models;
using API.Models.Dto;
using API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : Controller
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly ILogger<IdentityController> logger;
        protected ResponseDto _responseDto;
        protected ResponseAuthDto _responseAuthDto;
        public IdentityController(IIdentityRepository identityRepository, ILogger<IdentityController> logger)
        {
            _identityRepository = identityRepository;
            this.logger = logger;
            _responseDto = new ResponseDto();
            _responseAuthDto = new ResponseAuthDto();
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            logger.LogInformation($"Intentando autenticar usuario con el correo {model.Email}");

            try
            {
                var response = await _identityRepository.Login(model);

                if (response == "NotEmpty")
                {
                    logger.LogCritical("Ha ocurrido un error al autenticarse ni el usuario ni la contraseña  pueden ser vacíos.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha ocurrido un error al autenticarse ni el usuario ni la contraseña  pueden ser vacíos.";

                    return BadRequest(_responseDto);
                }

                if (response == "NoUser")
                {
                    logger.LogError($"El usuario con el correo {model.Email} no existe.");

                    _responseAuthDto.IsSuccess = false;
                    _responseAuthDto.Authenticated = false;
                    _responseAuthDto.DisplayMessage = $"El usuario no existe.";

                    return BadRequest(_responseAuthDto);
                }
                
                if (response == "Disabled")
                {
                    logger.LogError($"El usuario con el correo {model.Email} está deshabiliatdo.");

                    _responseAuthDto.IsSuccess = false;
                    _responseAuthDto.Authenticated = false;
                    _responseAuthDto.DisplayMessage = $"El usuario no está disponible.";

                    return BadRequest(_responseAuthDto);
                }

                if (response == "NoConfirm")
                {
                    logger.LogError($"El Usuario {model.Email} Todavía no ha Hecho la Confirmación del Usuario a través del correo.");

                    _responseAuthDto.IsSuccess = false;
                    _responseAuthDto.Authenticated = false;
                    _responseAuthDto.DisplayMessage = "Confirmar el acceso de registro que se le envió a su cuenta de correo.";

                    return BadRequest(_responseAuthDto);
                }

                if (response == "WrongPassword")
                {
                    logger.LogError($"El Usuario con el correo {model.Email} introdujo la Contraseña Incorrecta.");

                    _responseAuthDto.IsSuccess = false;
                    _responseAuthDto.Authenticated = false;
                    _responseAuthDto.DisplayMessage = "La combinación de usuario y contraseña no es correcta.";

                    return BadRequest(_responseAuthDto);
                }
                else
                {
                    logger.LogInformation($"El Usuario con el correo {model.Email} Inicio la Sesión.");

                    HttpContext.Session.SetString("userEmail", model.Email);
                    HttpContext.Session.SetString("token", response);
                    HttpContext.Session.SetString("isAuthenticated", "true");

                    _responseAuthDto.Token = response;
                    _responseAuthDto.Authenticated = true;
                    _responseAuthDto.Role = "Customer";
                    _responseAuthDto.DisplayMessage = "Usuario Conectado.";

                    return Ok(_responseAuthDto);
                }
            }
            catch (System.Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_responseDto);
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegisterDto model)
        {
            logger.LogInformation($"Intentando Registrar Usuario con el correo {model.Email}");

            try
            {
                var response = await _identityRepository.Register(model.Email, model.Password, model.ConfirmPassword);

                if (response == -3)
                {
                    logger.LogCritical("Ha ocurrido un error al registrarse ni el correo ni la contraseña  pueden ser vacíos.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha ocurrido un error al registrarse ni el correo ni la contraseña  pueden ser vacíos.";

                    return BadRequest(_responseDto);
                }

                if (response == -1)
                {
                    logger.LogError($"El usuario  ya existe.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "El usuario ya existe.";

                    return BadRequest(_responseDto);
                }

                if (response == -2)
                {
                    logger.LogError("Las Contraseñas son Difererentes.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Las contraseñas no coinciden.";

                    return BadRequest(_responseDto);
                }

                if (response == -4)
                {
                    logger.LogError("No se pudo Asociar el Usuario y el Rol.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "No se pudo asociar el usuario y el rol.";

                    return BadRequest(_responseDto);
                }

                if (response == -500)
                {
                    logger.LogError($"Error al Crear el Usuario con el correo {model.Email}.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Error al crear el usuario.";

                    return BadRequest(_responseDto);
                }

                if (response == -501)
                {
                    logger.LogError($"El Correo de Confirmación al Usuario {model.Email} no Puede ser Enviado.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "El correo de confirmación al usuario  no puede ser enviado.";

                    return BadRequest(_responseDto);
                }

                logger.LogInformation($"El Usuario  con el correo {model.Email} fue Creado con Éxito.");

                _responseDto.DisplayMessage = "Se le envió un correo con indicaciones para su registro.";
                _responseDto.Result = response;

                return Ok(_responseDto);
            }
            catch (System.Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_responseDto);
            }
        }
        
        [HttpPost("ForgetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword(ForgetPassword model)
        {
            logger.LogInformation($"Intentando Recuperar Contraseña del Usuario.");

            try
            {
                var response = await _identityRepository.ForgetPassword(model.Email);

                if (response == -2)
                {
                    logger.LogCritical("Ha ocurrido un error al recuperar la contraseña  el correo no puede ser vacío.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha ocurrido un error al recuperar la contraseña  el correo no puede ser vacío.";

                    return BadRequest(_responseDto);
                }

                if (response == -1)
                {
                    logger.LogError($"El usuario no existe.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "El usuario no existe.";

                    return BadRequest(_responseDto);
                }

                if (response == -500)
                {
                    logger.LogError($"Error al intentar recuperar la contraseña del usuario.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Error al intentar recuperar la contraseña del usuario.";

                    return BadRequest(_responseDto);
                }

                logger.LogInformation($"Se le envió un correo con indicaciones para recuperar su contraseña.");

                _responseDto.DisplayMessage = "Se le envió un correo con indicaciones para recuperar su contraseña. ";
                _responseDto.Result = response;

                return Ok(_responseDto);
            }
            catch (System.Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_responseDto);
            }
        }

        [HttpPost("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmail model)
        {
            logger.LogInformation($"Intentando confrimar registro del usuario con id {model.UserId}");

            try
            {
                var response = await _identityRepository.ConfirmEmail(model);

                if (response == "NotEmpty")
                {
                    logger.LogCritical("Ha ocurrido un error al confirmar el registro del usuario el id no puede ser vacío.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha ocurrido un error al confirmar el registro del usuario el id no puede ser vacío.";

                    return BadRequest(_responseDto);
                }
                
                if (response == "IsConfirmed")
                {
                    logger.LogCritical("El usuario ya ha sido habilitado.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "El usuario ya ha sido habilitado.";

                    return BadRequest(_responseDto);
                }

                if (response == "WrongConfirm")
                {
                    logger.LogInformation($"Ha ocurrido un error al confirmar el registro del usuario con id {model.UserId}");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha ocurrido un error al confirmar el registro del usuario.";

                    return BadRequest(_responseDto);
                }
                logger.LogInformation("La confirmación se ha realizado con éxito.");

                _responseDto.Result = response;
                _responseDto.DisplayMessage = "La confirmación se ha realizado con éxito.";

                return Ok(_responseDto);
            }
            catch (System.Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_responseDto);
            }
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            logger.LogInformation($"Intentando resetear la contraseña del usuario");

            try
            {
                var response = await _identityRepository.ResetPassword(model.Id, model.Password, model.ConfirmPassword);

                if (response == "NotEmpty")
                {
                    logger.LogCritical("Ha ocurrido un error al resetear la contraseña  ni el correo, ni la contraseña pueden ser vacío.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha ocurrido un error al resetear la contraseña  ni el correo, ni la contraseña pueden ser vacío.";

                    return BadRequest(_responseDto);
                }
                
                if (response == "NoExist")
                {
                    logger.LogCritical("Ha Ocurrido un Error al Resetear la Contraseña  el Usuario no Existe.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "El usuario no existe.";

                    return BadRequest(_responseDto);
                }

                if (response == "WrongResetPassword")
                {
                    logger.LogError($"Ha ocurrido un error al resetear la contraseña  del usuario.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha ocurrido un error al resetear la contraseña  del usuario.";

                    return BadRequest(_responseDto);
                }
                else
                {
                    logger.LogInformation($"La contraseña se ha cambiado con éxito");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = "La contraseña se ha cambiado con éxito";

                    return Ok(_responseDto);
                }
            }
            catch (System.Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_responseDto);
            }

        }

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(PasswordChangeDto model)
        {
            //var email = HttpContext.Session.GetString("userEmail");
            //var token = HttpContext.Session.GetString("token");
            //var isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            logger.LogInformation($"Intentando resetear la contraseña del usuario con el correo {model.Email}");

            try
            {
                var response = await _identityRepository.ChangePassword(model);

                if (response == "FieldNull")
                {
                    logger.LogCritical("Ha ocurrido un error al cambiar la contraseña  ninguno de los campos puede ser vacío.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha ocurrido un error al cambiar la contraseña  ninguno de los campos puede ser vacío.";

                    return BadRequest(_responseDto);
                }

                if (response == "NotExistUser")
                {
                    logger.LogError($"Ha ocurrido un error al cambiar la contraseña del usuario  con el correo {model.Email} no existe.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"Ha ocurrido un error al cambiar la contraseña del usuario  con el correo {model.Email} no existe.";
                }

                if (response == "DiferentPassword")
                {
                    logger.LogError($"Ha ocurrido un error al cambiar la contraseña del usuario  con el correo {model.Email} las contraseñas no coinciden.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"Ha Ocurrido un error las contraseñas no coinciden.";
                }

                if (response == "ErrorChangePassword")
                {
                    logger.LogError($"Ha Ocurrido un error al cambiar la contraseña.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"Ha Ocurrido un error al cambiar la contraseña.";

                    return BadRequest(_responseDto);
                }
                else
                {
                    logger.LogInformation($"La Contraseña del Usuario con el correo {model.Email} se ha Cambiado con Éxito.");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = $"La contraseña se ha cambiado satisfactoriamente.";

                    return Ok(_responseDto);
                }
            }
            catch (System.Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_responseDto);
            }

        }

        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout(string email)   
        {
            //var email = HttpContext.Session.GetString("userEmail");
            //var token = HttpContext.Session.GetString("token");
            //var isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            logger.LogInformation($"Intentando cerrar la sesión del usuario con el correo {email}");

            try
            {
                var response = await _identityRepository.Logout();

                if (response != "Closed")
                {
                    logger.LogCritical("Ha ocurrido un error al intentar cerrar la sesión.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Error al intentar cerrar sesión.";

                    return BadRequest(_responseDto);
                }
                else
                {
                    logger.LogInformation($"La sesión se ha cerrado satisfactoriamente.");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = $"La sesión se ha cerrado satisfactoriamente.";

                    return Ok(_responseDto);
                }
                //if (await _identityRepository.SuccessAuth(email, token, isAuthenticated))
                //{

                //}
                //else
                //{
                //    logger.LogCritical($"El Usuario con el Correo {email} no Está Autenticado en el Sistema");

                //    _responseDto.IsSuccess = false;
                //    _responseDto.DisplayMessage = $"El Usuario con el Correo {email} no Está Autenticado en el Sistema";
                //    return BadRequest(_responseDto);
                //}
            }
            catch (System.Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_responseDto);
            }

        }

        [HttpPost("DeleteUser")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteUser(string email)
        {
            logger.LogInformation($"Eliminar el usuario");

            try
            {
                var response = await _identityRepository.DeleteUser(email);

                if (response == false)
                {
                    logger.LogCritical("Ha ocurrido un error al eliminar el usuario.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha ocurrido un error al eliminar el usuario.";

                    return BadRequest(_responseDto);
                }
                else
                {
                    logger.LogInformation($"El usuario se ha eliminado satisfactoriamente.");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = "El usuario se ha eliminado satisfactoriamente.";

                    return Ok(_responseDto);
                }
            }
            catch (System.Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_responseDto);
            }

        }
    }
}
