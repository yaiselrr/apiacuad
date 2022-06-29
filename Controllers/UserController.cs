using API.Models;
using API.Models.Dto;
using API.Repository.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> logger;
        protected ResponseDto _responseDto;
        private readonly UserManager<GeneralDataUser> _userManager;
        public UserController(IUserRepository userRepository, ILogger<UserController> logger, UserManager<GeneralDataUser> userManager)
        {
            _userRepository = userRepository;
            this.logger = logger;
            _responseDto = new ResponseDto();
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeneralDataUser>>> ListUsers()
        {
            var email = HttpContext.Session.GetString("userEmail");
            var token = HttpContext.Session.GetString("token");
            var isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null && !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                logger.LogError($"401 El Usuario {email} no tiene suficientes privilegios para gestionar roles.");

                _responseDto.Result = 401;
                _responseDto.DisplayMessage = $"El Usuario {email} no tiene suficientes privilegios.";

                return BadRequest(_responseDto);
            }

            logger.LogInformation($"Intentando Obtener los Usuarios.");

            try
            {
                if (await _userRepository.SuccessAuth(email, token, isAuthenticated))
                {

                    var response = _userRepository.GetUsers();

                    if (response.Count() == 0)
                    {
                        logger.LogError("No hay Usuarios Disponibles.");

                        _responseDto.Result = response;
                        _responseDto.DisplayMessage = "No hay Usuarios Disponibles.";

                        return Ok(_responseDto);
                    }
                    else
                    {
                        logger.LogInformation($"Listado de Usuarios.");

                        _responseDto.Result = response;
                        _responseDto.DisplayMessage = $"Listado de Usuarios";

                        return Ok(_responseDto);
                    }

                }
                else
                {
                    logger.LogCritical($"El Usuario con el Correo {email} no Está Autenticado en el Sistema");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"El Usuario con el Correo {email} no Está Autenticado en el Sistema";
                    return BadRequest(_responseDto);
                }
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_responseDto);
            }


        }

        [HttpPost]
        public async Task<ActionResult<int>> AddUser(UserDto userDto)
        {
            var email = HttpContext.Session.GetString("userEmail");
            var token = HttpContext.Session.GetString("token");
            var isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null && !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                logger.LogError($"401 El Usuario {email} no tiene suficientes privilegios para gestionar roles.");

                _responseDto.Result = 401;
                _responseDto.DisplayMessage = $"El Usuario {email} no tiene suficientes privilegios.";

                return BadRequest(_responseDto);
            }

            logger.LogInformation($"Intentando Registrar Usuario con el correo {userDto.Email}");

            try
            {
                var response = await _userRepository.AddUser(userDto);

                if (response == -3)
                {
                    logger.LogCritical("Ha Ocurrido un Error al Registrarse ni el Correo ni la Contraseña  pueden ser vacío.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha Ocurrido un Error al Registrarse ni el Correo ni la Contraseña  pueden ser vacío.";

                    return BadRequest(_responseDto);
                }

                if (response == -1)
                {
                    logger.LogError($"El Usuario con el correo {userDto.Email} ya Existe.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "El Usuario ya Existe.";

                    return BadRequest(_responseDto);
                }

                if (response == -2)
                {
                    logger.LogError("Las Contraseñas son Difererentes.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Las Contraseñas son Difererentes.";

                    return BadRequest(_responseDto);
                }

                if (response == -4)
                {
                    logger.LogError("No se pudo Asociar el Usuario y el Rol.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "No se pudo Asociar el Usuario y el Rol.";

                    return BadRequest(_responseDto);
                }

                if (response == -500)
                {
                    logger.LogError($"Error al Crear el Usuario con el correo {userDto.Email}.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Error al Crear el Usuario.";

                    return BadRequest(_responseDto);
                }

                if (response == -501)
                {
                    logger.LogError($"El Correo de Confirmación al Usuario {userDto.Email} no Puede ser Enviado.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "El Correo de Confirmación al Usuario  no Puede ser Enviado.";

                    return BadRequest(_responseDto);
                }

                logger.LogInformation($"El Usuario  con el correo {userDto.Email} fue Creado con Éxito.");

                _responseDto.DisplayMessage = "El Usuario fue Creado con Éxito.";
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

        [HttpGet("GetDataUser")]
        public async Task<ActionResult<GeneralDataUser>> GetDataUser()
        {
            var email = HttpContext.Session.GetString("userEmail");
            var token = HttpContext.Session.GetString("token");
            var isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            try
            {
                if (await _userRepository.SuccessAuth(email, token, isAuthenticated))
                {
                    logger.LogInformation($"Intentando Obtener los Datos Generales del Usuario con el correo {email}");

                    var response = await _userRepository.GetDataUser(email);

                    if (response == null)
                    {
                        logger.LogError($"El Usuario con el correo {email} no Existe o no Ha confirmado su registro aún.");

                        _responseDto.IsSuccess = false;
                        _responseDto.DisplayMessage = $"El Usuario con el correo {email} no Existe o no Ha confirmado su registro aún.";

                        return BadRequest(_responseDto);
                    }
                    else
                    {
                        logger.LogInformation($"Datos del Usuario con el correo {email}.");

                        _responseDto.Result = response;
                        _responseDto.DisplayMessage = $"Datos del Usuario con el correo {email}.";

                        return Ok(_responseDto);
                    }
                }
                else
                {
                    logger.LogCritical($"El Usuario con el correo {email} no Está Autenticado Correctamente.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"El Usuario con el correo {email} no Está Autenticado Correctamente.";

                    return BadRequest(_responseDto);

                }
            }
            catch (System.Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_responseDto);
            }
        }

        [HttpPut]
        public async Task<ActionResult<string>> UpdateUser(UserUpdateDto userUpdateDto)
        {
            var email = HttpContext.Session.GetString("userEmail");
            var token = HttpContext.Session.GetString("token");
            var isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null && !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                logger.LogError($"401 El Usuario {email} no tiene suficientes privilegios para gestionar roles.");

                _responseDto.Result = 401;
                _responseDto.DisplayMessage = $"El Usuario {email} no tiene suficientes privilegios.";

                return BadRequest(_responseDto);
            }

            try
            {
                if (await _userRepository.SuccessAuth(email, token, isAuthenticated))
                {
                    logger.LogInformation($"Intentando Editar un Usuario");

                    var response = await _userRepository.UpdateUser(userUpdateDto);

                    if (response == "IsRepeated")
                    {
                        logger.LogError("Error al Intentar Editar el Usuario los Números de Teléfonos ya existen.");

                        _responseDto.IsSuccess = false;
                        _responseDto.DisplayMessage = "Error al Intentar Editar el Usuario los Números de Teléfonos ya existen.";

                        return BadRequest(_responseDto);
                    }

                    if (response == "WrongUserEdit")
                    {
                        logger.LogError("Error al Intentar Editar el Usuario.");

                        _responseDto.IsSuccess = false;
                        _responseDto.DisplayMessage = "Error al Intentar Editar el Usuario.";

                        return BadRequest(_responseDto);
                    }
                    else
                    {
                        logger.LogInformation("El Usuario fue Editado Satisfactoriamente.");

                        _responseDto.Result = response;
                        _responseDto.DisplayMessage = "El Usuario fue Editado Satisfactoriamente.";

                        return Ok(_responseDto);
                    }
                }
                else
                {
                    logger.LogCritical($"El Usuario con el correo {email} no Está Autenticado Correctamente o no Existe.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"El Usuario con el correo {email} no Está Autenticado Correctamente o no Existe.";

                    return BadRequest(_responseDto);
                }
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_responseDto);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<string>> DeleteUser(string id)
        {
            var email = HttpContext.Session.GetString("userEmail");
            var token = HttpContext.Session.GetString("token");
            var isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null && !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                logger.LogError($"401 El Usuario {email} no tiene suficientes privilegios para gestionar roles.");

                _responseDto.Result = 401;
                _responseDto.DisplayMessage = $"El Usuario {email} no tiene suficientes privilegios.";

                return BadRequest(_responseDto);
            }

            try
            {
                if (await _userRepository.SuccessAuth(email, token, isAuthenticated))
                {
                    logger.LogInformation($"Intentando Eliminar un Usuario por el Id.");

                    var response = await _userRepository.DeleteUser(id);

                    if (response == "UserNoExist")
                    {
                        logger.LogError("Error al Intentar Eliminar el Usuario el Mismo no Existe.");

                        _responseDto.IsSuccess = false;
                        _responseDto.DisplayMessage = "Error al Intentar Eliminar el Usuario el Mismo no Existe.";

                        return BadRequest(_responseDto);
                    }
                    else
                    {
                        logger.LogInformation("El Usuario Fue Eliminado Satisfactoriamente.");

                        _responseDto.Result = response;
                        _responseDto.DisplayMessage = "El Usuario Fue Eliminado Satisfactoriamente.";

                        return Ok(_responseDto);
                    }
                }
                else
                {
                    logger.LogCritical($"El Usuario con el correo {email} no Está Autenticado Correctamente o no Existe.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"El Usuario con el correo {email} no Está Autenticado Correctamente o no Existe.";

                    return BadRequest(_responseDto);
                }
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_responseDto);
            }
        }
    }
}
