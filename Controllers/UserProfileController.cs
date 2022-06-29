using API.Models;
using API.Models.Dto;
using API.Repository.Services.GeneralDataUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly IGeneralDataUserRepository _generalDataUserRepository;
        private readonly ILogger<UserProfileController> logger;
        protected ResponseDto _responseDto;

        public UserProfileController(IGeneralDataUserRepository generalDataUserRepository, ILogger<UserProfileController> logger)
        {
            _generalDataUserRepository = generalDataUserRepository;
            this.logger = logger;
            _responseDto = new ResponseDto();
        }

        [HttpGet("DataProfile")]
        public async Task<IActionResult> GetGeneralDataUser(string email)
        {
            //var email = HttpContext.Session.GetString("userEmail");
            //var token = HttpContext.Session.GetString("token");
            //var isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            try
            {
                logger.LogInformation($"Intentando Obtener los Datos Generales del Usuario con el correo {email}");

                var response = await _generalDataUserRepository.GetGeneralDataUser(email);

                if (response == null)
                {
                    logger.LogError($"El usuario con el correo {email} no existe o no ha confirmado su registro aún.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"El usuario con el correo {email} no existe o no Ha confirmado su registro aún.";

                    return BadRequest(_responseDto);
                }
                else
                {
                    logger.LogInformation($"Datos del Usuario con el correo {email}.");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = $"Datos del usuario con el correo {email}.";

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

        [HttpPost("DataProfile")]
        public async Task<IActionResult> PostGeneralDataUser(GeneralDataUserDto model)
        {
            //var email = HttpContext.Session.GetString("userEmail");
            //var token = HttpContext.Session.GetString("token");
            //var isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            try
            {
                logger.LogInformation($"Intentando Editar los Datos Generales del Usuario con el Correo {model.Email}");

                var response = await _generalDataUserRepository.PostGeneralDataUser(model);

                if (response == "IsRepeated")
                {
                    logger.LogError($"Error al Intentar Editar los Datos Generales del Usuario con el Correo {model.Email} ya Existen Otros con los Mismos Números Telefónicos.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"Error al intentar editar los datos generales del usuario con el correo {model.Email} ya existen otros con los mismos números telefónicos.";

                    return BadRequest(_responseDto);
                }

                if (response == "WrongUserEdit")
                {
                    logger.LogError($"Error al Intentar Editar los Datos Generales del Usuario con el Correo {model.Email}.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"Error al intentar editar los datos.";

                    return BadRequest(_responseDto);
                }
                else
                {
                    logger.LogInformation($"Los Datos del Usuario con el Correo {model.Email} Fueron Agregados Satisfactoriamente.");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = $"Los datos fueron agregados satisfactoriamente.";

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
