using API.Models;
using API.Models.Dto;
using API.Repository.Services.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class PropertyController : Controller
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly ILogger<PropertyController> logger;
        protected ResponseDto _responseDto;

        public PropertyController(IPropertyRepository propertyRepository, ILogger<PropertyController> logger)
        {
            _propertyRepository = propertyRepository;

            this.logger = logger;

            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Property>>> ListProperties(string email)
        {
            //var email = HttpContext.Session.GetString("userEmail");
            //var token = HttpContext.Session.GetString("token");
            //var isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            logger.LogInformation($"Intentando Obtener los Inmuebles del Usuario con la Cuenta de Correo {email}");

            try
            {
                var response = await _propertyRepository.GetProperties(email);

                if (response == null)
                {
                    logger.LogError("El usuario no existe.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "El usuario no existe.";

                    return BadRequest(_responseDto);
                }

                if (response.Count() == 0)
                {
                    logger.LogError("No hay inmuebles disponibles.");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = "No hay inmuebles disponibles.";

                    return Ok(_responseDto);
                }
                else
                {
                    logger.LogInformation($"Listado de Inmuebles del Usuario con la Cuenta de Correo {email}");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = $"Listado de Inmuebles.";

                    return Ok(_responseDto);
                }
                //if (await _propertyRepository.SuccessAuth(email, token, isAuthenticated))
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
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_responseDto);
            }

        }

        [HttpPost]
        public async Task<ActionResult<Property>> AddProperty(PropertyDto propertyDto)
        {
            //var email = HttpContext.Session.GetString("userEmail");
            //var token = HttpContext.Session.GetString("token");
            //var isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            try
            {
                logger.LogInformation($"Intentando Agregar un Inmueble");

                var response = await _propertyRepository.CreateProperty(propertyDto);

                if (response == "NoExistProperty")
                {
                    logger.LogError($"Error al Intentar Crear el Inmueble no existe el mismo.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"El inmueble no existe.";

                    return BadRequest(_responseDto);
                }

                if (response == "ExistProperty")
                {
                    logger.LogError($"Error al Intentar Crear el Inmueble no existe el mismo.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"El inmueble ya fue dado de alta.";

                    return BadRequest(_responseDto);
                }
                else
                {
                    logger.LogInformation("El Inmueble fue Agregado Satisfactoriamente.");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = "El inmueble fue agregado satisfactoriamente.";

                    return Ok(_responseDto);
                }
                //if (await _propertyRepository.SuccessAuth(email, token, isAuthenticated))
                //{

                //}
                //else
                //{
                //    logger.LogCritical($"El Usuario con el correo {email} no Está Autenticado Correctamente o no Existe.");

                //    _responseDto.IsSuccess = false;
                //    _responseDto.DisplayMessage = $"El Usuario con el correo {email} no Está Autenticado Correctamente o no Existe.";

                //    return BadRequest(_responseDto);
                //}
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_responseDto);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Property>> UpdateProperty(PropertyUpdateDto propertyUpdateDto)
        {
            //var email = HttpContext.Session.GetString("userEmail");
            //var token = HttpContext.Session.GetString("token");
            //var isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            try
            {
                //if (await _propertyRepository.SuccessAuth(email, token, isAuthenticated))
                //{

                //}
                //else
                //{
                //    logger.LogCritical($"El Usuario con el correo {email} no Está Autenticado Correctamente o no Existe.");

                //    _responseDto.IsSuccess = false;
                //    _responseDto.DisplayMessage = $"El Usuario con el correo {email} no Está Autenticado Correctamente o no Existe.";

                //    return BadRequest(_responseDto);
                //}

                logger.LogInformation($"Intentando Editar un Inmueble con la Cuenta de Correo {propertyUpdateDto.Email}");

                var response = await _propertyRepository.UpdateProperty(propertyUpdateDto.Email, propertyUpdateDto);

                if (response == "IsRepeat")
                {
                    logger.LogError($"Error al Intentar Editar el Inmueble con la Cuenta de Correo {propertyUpdateDto.Email} el Mismo no Puede Estar Repetido.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"Error al Intentar Editar el Inmueble con la Cuenta de Correo {propertyUpdateDto.Email} el Mismo no Puede Estar Repetido.";

                    return BadRequest(_responseDto);
                }

                if (response == "NoProperty")
                {
                    logger.LogError($"Error al Intentar Editar el Inmueble con la Cuenta de Correo {propertyUpdateDto.Email} la Misma no Existe.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"Error al Intentar Editar el Inmueble con la Cuenta de Correo {propertyUpdateDto.Email} la Misma no Existe.";

                    return BadRequest(_responseDto);
                }
                else
                {
                    logger.LogInformation("El Inmueble fue Editado Satisfactoriamente.");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = "El Inmueble fue Editado Satisfactoriamente.";

                    return Ok(_responseDto);
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
        public async Task<ActionResult<Property>> GetPropertyById(string id)
        {
            var email = HttpContext.Session.GetString("userEmail");
            var token = HttpContext.Session.GetString("token");
            var isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            try
            {
                if (await _propertyRepository.SuccessAuth(email, token, isAuthenticated))
                {
                    logger.LogInformation($"Intentando Obtener un Inmueble por el Id con la Cuenta de Correo {email}");

                    var response = await _propertyRepository.GetPropertyById(id, email);

                    if (response == null)
                    {
                        logger.LogError($"El Inmueble no Existe para la Cuenta de Correo {email}.");

                        _responseDto.IsSuccess = false;
                        _responseDto.DisplayMessage = $"El inmueble no existe para la cuenta de correo {email}.";

                        return BadRequest(_responseDto);
                    }
                    else
                    {
                        logger.LogInformation("Los Datos del Inmueble fueron Obtenidos Satisfactoriamente.");

                        _responseDto.Result = response;
                        _responseDto.DisplayMessage = "Los datos del inmueble fueron obtenidos satisfactoriamente.";

                        return Ok(_responseDto);
                    }
                }
                else
                {
                    logger.LogCritical($"El Usuario con el correo {email} no Está Autenticado Correctamente o no Existe.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"El usuario con el correo {email} no está autenticado correctamente o no existe.";

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

        [HttpPost("DeleteByProperty")]
        public async Task<ActionResult<Property>> DeleteProperty(PropertyDeleteDto model)
        {
            //var email = HttpContext.Session.GetString("userEmail");
            //var token = HttpContext.Session.GetString("token");
            //var isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            try
            {
                logger.LogInformation($"Intentando Eliminar una Tarjeta de Crédito por el Id con la Cuenta de Correo {model.Email}");

                var response = await _propertyRepository.DeleteProperty(model.Email, model.Id);

                if (response == "NoProperty")
                {
                    logger.LogError($"Error al Intentar Eliminar el Inmueble con la Cuenta de Correo {model.Email} la Misma no Existe.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"Error al intentar eliminar el inmueble con la cuenta de correo {model.Email} la misma no existe.";

                    return BadRequest(_responseDto);
                }
                else
                {
                    logger.LogInformation("El Inmueble fue eliminado satisfactoriamente.");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = "El inmueble fue eliminado satisfactoriamente.";

                    return Ok(_responseDto);
                }

                //if (await _propertyRepository.SuccessAuth(email, token, isAuthenticated))
                //{

                //}
                //else
                //{
                //    logger.LogCritical($"El Usuario con el correo {email} no Está Autenticado Correctamente o no Existe.");

                //    _responseDto.IsSuccess = false;
                //    _responseDto.DisplayMessage = $"El Usuario con el correo {email} no Está Autenticado Correctamente o no Existe.";

                //    return BadRequest(_responseDto);
                //}
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_responseDto);
            }
        }

        [HttpPost("AuthorizeUser")]
        public async Task<IActionResult> AuthorizeUser(string email, string userId, string propertyId)
        {
            logger.LogInformation("Intentando Autorizar uso del Inmueble");

            try
            {
                var response = await _propertyRepository.AuthorizeUser(email, userId, propertyId);

                if (response == "NotEmpty")
                {
                    logger.LogCritical("Ha Ocurrido un Error al Autorizar uso del Inmueble el id no puede ser vacío.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha ocurrido un error al autorizar uso del inmueble el id no puede ser vacío.";

                    return BadRequest(_responseDto);
                }
                
                if (response == "UserNoExist")
                {
                    logger.LogCritical("Ha Ocurrido un Error al Autorizar uso del Inmueble el Usuario no existe.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha ocurrido un error al autorizar uso del inmueble el usuario no existe.";

                    return BadRequest(_responseDto);
                }

                if (response == "Error")
                {
                    logger.LogInformation($"Ha Ocurrido un Error al Autorizar uso del Inmueble");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha ocurrido un error al autorizar uso del inmueble";

                    return BadRequest(_responseDto);
                }
                else
                {
                    logger.LogInformation("La Autorización del uso del Inmueble se ha Realizado con Éxito.");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = "La autorización del uso del inmueble fue satisfactoria.";

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

        [HttpPost("NoAuthorizeUser")]
        public async Task<IActionResult> NoAuthorizeUser(string email, string propertyId, string userId)
        {
            logger.LogInformation("Intentando no Autorizar uso del Inmueble");

            try
            {
                var response = await _propertyRepository.NoAuthorizeUser(email, propertyId, userId);

                if (response == "NoSendMail")
                {
                    logger.LogCritical("Ha Ocurrido un Error al No Autorizar uso del Inmueble el no se pudo enviar.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha ocurrido un error al no autorizar uso del inmueble el no se pudo enviar.";

                    return BadRequest(_responseDto);
                }

                /*if (response == "UserNoExist")
                {
                    logger.LogCritical("Ha Ocurrido un Error al Autorizar uso del Inmueble el Usuario no existe.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha Ocurrido un Error al Autorizar uso del Inmueble el Usuario no existe.";

                    return BadRequest(_responseDto);
                }*/

                if (response == "Error")
                {
                    logger.LogInformation($"Ha Ocurrido un Error al no Autorizar uso del Inmueble");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha ocurrido un error al no autorizar uso del inmueble";

                    return BadRequest(_responseDto);
                }
                else
                {
                    logger.LogInformation("La Autorización del uso del Inmueble se ha Realizado con Éxito.");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = "La no autorización del uso del inmueble fue satisfactoria.";

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

        [HttpPost("TransferProperty")]
        public async Task<IActionResult> TransferProperty(TransferPropertyDto model)
        {
            logger.LogInformation("Intentando no Autorizar uso del Inmueble");

            try
            {
                var response = await _propertyRepository.TransferProperty(model);

                if (response == "NoSendMail")
                {
                    logger.LogCritical("Ha Ocurrido un Error al No Autorizar uso del Inmueble el no se pudo enviar.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha ocurrido un error al no autorizar uso del inmueble el no se pudo enviar.";

                    return BadRequest(_responseDto);
                }

                if (response == "UserOwnerNoExist")
                {
                    logger.LogCritical("Ha Ocurrido un Error al Intentar Transferir el Inmueble el Usuario Propietario no existe.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha ocurrido un error al intentar transferir el inmueble el usuario propietario no existe.";

                    return BadRequest(_responseDto);
                }

                if (response == "userTransferNoExist")
                {
                    logger.LogCritical("Ha Ocurrido un Error al Intentar Transferir el Inmueble el Usuario no Propietario no existe.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha ocurrido un error al intentar transferir el inmueble el usuario no propietario no existe.";

                    return BadRequest(_responseDto);
                }

                if (response == "PropertyNoExist")
                {
                    logger.LogInformation($"Ha Ocurrido un Error Intentar Transferir el Inmueble el mismo no Existe en propiedad del Usuario");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha ocurrido un error al intentar transferir el inmueble el mismo no existe en propiedad del usuario";

                    return BadRequest(_responseDto);
                }

                if (response == "NoPropertyNoExist")
                {
                    logger.LogInformation($"Ha Ocurrido un Error Intentar Transferir el Inmueble el mismo no se uso por Usuario");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Ha ocurrido un error al intentar transferir el inmueble el mismo no se uso por usuario";

                    return BadRequest(_responseDto);
                }
                else
                {
                    logger.LogInformation("La Transferencia del Inmueble se ha Realizado con Éxito.");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = "La transferencia del inmueble fue éxitosa.";

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

        [HttpGet("GetBalance")]
        public async Task<ActionResult<IEnumerable<BalanceDto>>> GetBalance(string id) 
        {
            try
            {
                logger.LogInformation($"Intentando Obtener saldo de un Inmueble");

                var response = _propertyRepository.GetBalance(Int32.Parse(id));

                if (response == null)
                {
                    logger.LogError($"El Inmueble no Existe.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"El Inmueble no Existe.";

                    return BadRequest(_responseDto);
                }
                else
                {
                    logger.LogInformation("El saldo del Inmueble fue Obtenido Satisfactoriamente.");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = "El saldo del inmueble fue obtenido satisfactoriamente.";

                    return Ok(_responseDto);
                }
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_responseDto);
            }
        }

        
        [HttpPost("DeletePropertyEnd")]
        [AllowAnonymous]
        public async Task<ActionResult<Property>> DeletePropertyEnd(string id)
        {

            try
            {
                logger.LogInformation($"Intentando Eliminar una Tarjeta de Crédito por el Id con la Cuenta de Correo {id}");

                var response = await _propertyRepository.DeletePropertyEnd(id);

                if (response == "NoProperty")
                {
                    logger.LogError($"Error al Intentar Eliminar el Inmueble con la Cuenta de Correo {id} la Misma no Existe.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"Error al intentar eliminar el inmueble con la cuenta de correo {id} la misma no existe.";

                    return BadRequest(_responseDto);
                }
                else
                {
                    logger.LogInformation("El Inmueble fue eliminado satisfactoriamente.");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = "El inmueble fue eliminado satisfactoriamente.";

                    return Ok(_responseDto);
                }
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_responseDto);
            }
        }

        [HttpGet("GetPropertiesNoMain")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<string>>> GetPropertiesNoMain(string idProperty)
        {

            logger.LogInformation($"Intentando Obtener los Inmuebles no principales.");

            try
            {
                var response = await _propertyRepository.GetPropertiesNoMain(idProperty);

                if (response.Count() == 0)
                {
                    logger.LogError("No hay inmuebles disponibles.");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = "No hay inmuebles disponibles.";

                    return Ok(_responseDto);
                }
                else
                {
                    logger.LogInformation($"Listado de Inmuebles del Usuario");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = $"Listado de Inmuebles.";

                    return Ok(_responseDto);
                }
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_responseDto);
            }

        }

        [HttpGet("GetPropertiesMain")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Property>>> GetPropertiesMain(string email)
        {

            logger.LogInformation($"Intentando Obtener los Inmuebles del Usuario con la Cuenta de Correo {email}");

            try
            {
                var response = await _propertyRepository.GetPropertiesMain(email);

                if (response == null)
                {
                    logger.LogError("El usuario no existe.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "El usuario no existe.";

                    return BadRequest(_responseDto);
                }

                if (response.Count() == 0)
                {
                    logger.LogError("No hay inmuebles disponibles.");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = "No hay inmuebles disponibles.";

                    return Ok(_responseDto);
                }
                else
                {
                    logger.LogInformation($"Listado de Inmuebles del Usuario con la Cuenta de Correo {email}");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = $"Listado de Inmuebles.";

                    return Ok(_responseDto);
                }
                //if (await _propertyRepository.SuccessAuth(email, token, isAuthenticated))
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
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_responseDto);
            }

        }
    }
}
