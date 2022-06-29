using API.Models;
using API.Models.Dto;
using API.Repository.Services.Roles;
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
    public class RolesController : Controller
    {
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<RolesController> logger;
        protected ResponseDto _responseDto;
        private readonly UserManager<GeneralDataUser> _userManager;

        public RolesController(IRoleRepository roleRepository, ILogger<RolesController> logger, UserManager<GeneralDataUser> userManager)
        {
            _roleRepository = roleRepository;
            this.logger = logger;
            _responseDto = new ResponseDto();
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> ListRoles()
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

            logger.LogInformation($"Intentando Obtener los Roles.");

            try
            {
                if (await _roleRepository.SuccessAuth(email, token, isAuthenticated))
                {

                    var response = await _roleRepository.GetRoles();

                    if (response.Count() == 0)
                    {
                        logger.LogError("No hay Roles Disponibles.");

                        _responseDto.Result = response;
                        _responseDto.DisplayMessage = "No hay Roles Disponibles.";

                        return Ok(_responseDto);
                    }
                    else
                    {
                        logger.LogInformation($"Listado de Roles.");

                        _responseDto.Result = response;
                        _responseDto.DisplayMessage = $"Listado de Roles";

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
        public async Task<ActionResult<RoleDto>> AddRole(RoleDto roleDto)
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
                if (await _roleRepository.SuccessAuth(email, token, isAuthenticated))
                {
                    logger.LogInformation($"Intentando Agregar un Rol");

                    var response = await _roleRepository.CreateRole(roleDto);

                    if (response == "NotEmpty")
                    {
                        logger.LogError("Error al Intentar Crear el Rol Rellene el Campo Nombre Role.");

                        _responseDto.IsSuccess = false;
                        _responseDto.DisplayMessage = "Error al Intentar Crear el Rol Rellene el Campo Nombre Role.";

                        return BadRequest(_responseDto);
                    }

                    if (response == "ExistCard")
                    {
                        logger.LogError("Error al Intentar Crear el Rol ya Existe una Igual.");

                        _responseDto.IsSuccess = false;
                        _responseDto.DisplayMessage = "Error al Intentar Crear el Rol ya Existe una Igual.";

                        return BadRequest(_responseDto);
                    }

                    if (response == "Error")
                    {
                        logger.LogError("Error al Intentar Crear el Rol.");

                        _responseDto.IsSuccess = false;
                        _responseDto.DisplayMessage = "Error al Intentar Crear el Rol.";

                        return BadRequest(_responseDto);
                    }
                    else
                    {
                        logger.LogInformation("El Rol fue Agregado Satisfactoriamente.");

                        _responseDto.Result = response;
                        _responseDto.DisplayMessage = "El Rol fue Agregado Satisfactoriamente.";

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

        [HttpPut]
        public async Task<ActionResult<RoleUpdateDto>> UpdateRole(RoleUpdateDto _roleUpdateDto)
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
                if (await _roleRepository.SuccessAuth(email, token, isAuthenticated))
                {
                    logger.LogInformation($"Intentando Editar un Rol");

                    var response = await _roleRepository.UpdateRole(_roleUpdateDto);

                    if (response == "NotEmpty")
                    {
                        logger.LogError("Error al Intentar Editar el Rol los Campos no Pueden Estar Vacios.");

                        _responseDto.IsSuccess = false;
                        _responseDto.DisplayMessage = "Error al Intentar Editar el Rol los Campos no Pueden Estar Vacios.";

                        return BadRequest(_responseDto);
                    }

                    if (response == "NoExist")
                    {
                        logger.LogError("Error al Intentar Editar el Rol el Mismo no Existe.");

                        _responseDto.IsSuccess = false;
                        _responseDto.DisplayMessage = "Error al Intentar Editar el Rol el Mismo no Existe.";

                        return BadRequest(_responseDto);
                    }
                    else
                    {
                        logger.LogInformation("El Rol fue Editado Satisfactoriamente.");

                        _responseDto.Result = response;
                        _responseDto.DisplayMessage = "El Rol fue Editado Satisfactoriamente.";

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

        [HttpGet("GetById")]
        public async Task<ActionResult<IdentityRole>> GetRoleById(string id)
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
                if (await _roleRepository.SuccessAuth(email, token, isAuthenticated))
                {
                    logger.LogInformation("Intentando Obtener un Rol por el Id.");

                    var response = await _roleRepository.GetRoleById(id);

                    if (response == null)
                    {
                        logger.LogError("El rol no Existe.");

                        _responseDto.IsSuccess = false;
                        _responseDto.DisplayMessage = "El rol no Existe.";

                        return BadRequest(_responseDto);
                    }
                    else
                    {
                        logger.LogInformation("Los Datos del Rol fueron Obtenidos Satisfactoriamente.");

                        _responseDto.Result = response;
                        _responseDto.DisplayMessage = "Los Datos del Rol fueron Obtenidos Satisfactoriamente.";

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
        public async Task<ActionResult<string>> DeleteRole(string id)
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
                if (await _roleRepository.SuccessAuth(email, token, isAuthenticated))
                {
                    logger.LogInformation($"Intentando Eliminar un Rol por el Id.");

                    var response = await _roleRepository.DeleteRole(id);

                    if (response == "NoRol")
                    {
                        logger.LogError("Error al Intentar Eliminar el Rol el Mismo no Existe.");

                        _responseDto.IsSuccess = false;
                        _responseDto.DisplayMessage = "Error al Intentar Eliminar el Rol el Mismo no Existe.";

                        return BadRequest(_responseDto);
                    }
                    else
                    {
                        logger.LogInformation("El Rol Fue Eliminado Satisfactoriamente.");

                        _responseDto.Result = response;
                        _responseDto.DisplayMessage = "El Rol Fue Eliminado Satisfactoriamente.";

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

        [HttpPost("JoinUserRole")]
        public async Task<ActionResult<RoleDto>> PostJoinUserRole(JoinUserRoleDto joinUserRoleDto)
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
                if (await _roleRepository.SuccessAuth(email, token, isAuthenticated))
                {
                    logger.LogInformation($"Intentando Asociar Usuario con un Rol");

                    var response = await _roleRepository.JoinUserRole(joinUserRoleDto.IdRole, joinUserRoleDto.IdUser);

                    if (response == "NoRole")
                    {
                        logger.LogError("Error al Intentar Asociar Usuario con un Rol el Rol no Existe.");

                        _responseDto.IsSuccess = false;
                        _responseDto.DisplayMessage = "Error al Intentar Asociar Usuario con un Rol el Rol no Existe.";

                        return BadRequest(_responseDto);
                    }

                    if (response == "NoUser")
                    {
                        logger.LogError("Error al Intentar Asociar Usuario con un Rol el Usuario no Existe.");

                        _responseDto.IsSuccess = false;
                        _responseDto.DisplayMessage = "Error al Intentar Asociar Usuario con un Rol el Usuario no Existe.";

                        return BadRequest(_responseDto);
                    }

                    if (response == "Error")
                    {
                        logger.LogError("Error al Intentar Asociar Usuario con un Rol.");

                        _responseDto.IsSuccess = false;
                        _responseDto.DisplayMessage = "Error al Intentar Asociar Usuario con un Rol.";

                        return BadRequest(_responseDto);
                    }
                    else
                    {
                        logger.LogInformation("El Rol y el Usuario fueron Asociados Satisfactoriamente.");

                        _responseDto.Result = response;
                        _responseDto.DisplayMessage = "El Rol y el Usuario fueron Asociados Satisfactoriamente.";

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
