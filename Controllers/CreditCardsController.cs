using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using API.Models.Dto;
using API.Repository.Services.GeneralDataUsers;
using API.Repository.CreditCards;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CreditCardsController : Controller
    {
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly ILogger<IdentityController> logger;
        protected ResponseDto _responseDto;

        public CreditCardsController(ICreditCardRepository creditCardRepository, ILogger<IdentityController> logger)
        {
            _creditCardRepository = creditCardRepository;

            this.logger = logger;

            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreditCard>>> ListCreditCards(string email)
        {
            //var email = HttpContext.Session.GetString("userEmail");
            //var token = HttpContext.Session.GetString("token");
            //var isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            logger.LogInformation($"Intentando Obtener las tarjetas de Crédito del Usuario con la Cuenta de Correo {email}");

            var response = await _creditCardRepository.GetCreditCards(email);

            if (response == null)
            {
                logger.LogError("El Usuario No existe en el Sistema");

                _responseDto.IsSuccess = false;
                _responseDto.DisplayMessage = "El Usuario no existe en el Sistema";

                return BadRequest(_responseDto);
            }
            if (response.Count() == 0)
            {
                logger.LogError("No hay Tarjetas de Crédito Disponibles.");

                _responseDto.Result = response;
                _responseDto.DisplayMessage = "No hay Tarjetas de Crédito Disponibles.";

                return Ok(_responseDto);
            }
            else
            {
                logger.LogInformation($"Listado de Tarjetas de Crédito del Usuario con la Cuenta de Correo {email}");

                _responseDto.Result = response;
                _responseDto.DisplayMessage = $"Listado de Tarjetas de Crédito del Usuario con la Cuenta de Correo {email}";

                return Ok(_responseDto);
            }

            //try
            //{
            //    if (await _creditCardRepository.SuccessAuth(email, token, isAuthenticated))
            //    {
                    

            //    }
            //    else
            //    {
            //        logger.LogCritical($"El Usuario con el Correo {email} no Está Autenticado en el Sistema");

            //        _responseDto.IsSuccess = false;
            //        _responseDto.DisplayMessage = $"El Usuario con el Correo {email} no Está Autenticado en el Sistema";
            //        return BadRequest(_responseDto);
            //    }
            //}
            //catch (Exception ex)
            //{

            //    _responseDto.IsSuccess = false;
            //    _responseDto.ErrorMessage = new List<string> { ex.ToString() };
            //    return BadRequest(_responseDto);
            //}

            
        }

        [HttpPost]
        public async Task<ActionResult<CreditCard>> AddCreditCard(CreditCardDto creditCardDto)
        {
            //var email = HttpContext.Session.GetString("userEmail");
            //var token = HttpContext.Session.GetString("token");
            //var isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            try
            {
                //if (await _creditCardRepository.SuccessAuth(email, token, isAuthenticated))
                //{

                //}
                //else
                //{
                //    logger.LogCritical($"El Usuario con el correo {email} no Está Autenticado Correctamente o no Existe.");

                //    _responseDto.IsSuccess = false;
                //    _responseDto.DisplayMessage = $"El Usuario con el correo {email} no Está Autenticado Correctamente o no Existe.";

                //    return BadRequest(_responseDto);
                //}
                logger.LogInformation($"Intentando Agregar una Tarjeta de Crédito con la Cuenta de Correo {creditCardDto.Email}");

                var response = await _creditCardRepository.CreateCreditCard(creditCardDto);

                if (response == "ExistCard")
                {
                    logger.LogError($"Error al Intentar Crear la Tarjeta de Crédito con la Cuenta de Correo {creditCardDto.Email} ya Existe una Igual.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"Error al Intentar Crear la Tarjeta de Crédito con la Cuenta de Correo {creditCardDto.Email} ya Existe una Igual.";

                    return BadRequest(_responseDto);
                }
                else
                {
                    logger.LogInformation("La Tarjeta de Crédito fue Agregada Satisfactoriamente.");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = "La Tarjeta de Crédito fue Agregada Satisfactoriamente.";

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

        [HttpPut]
        public async Task<ActionResult<CreditCard>> UpdateCreditCard(CreditCardUpdateDto creditCardUpdateDto)
        {
            var email = HttpContext.Session.GetString("userEmail");
            var token = HttpContext.Session.GetString("token");
            var isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            try
            {
                if (await _creditCardRepository.SuccessAuth(email, token, isAuthenticated))
                {
                    logger.LogInformation($"Intentando Editar una Tarjeta de Crédito con la Cuenta de Correo {email}");

                    var response = await _creditCardRepository.UpdateCreditCard(email, creditCardUpdateDto);

                    if (response == "IsRepeat")
                    {
                        logger.LogError($"Error al Intentar Editar la Tarjeta de Crédito con la Cuenta de Correo {email} la Misma no Puede Estar Repetida.");

                        _responseDto.IsSuccess = false;
                        _responseDto.DisplayMessage = $"Error al Intentar Crear la Tarjeta de Crédito con la Cuenta de Correo {email} la Misma no no Puede Estar Repetida.";

                        return BadRequest(_responseDto);
                    }

                    if (response == "NoCard")
                    {
                        logger.LogError($"Error al Intentar Editar la Tarjeta de Crédito con la Cuenta de Correo {email} la Misma no Existe.");

                        _responseDto.IsSuccess = false;
                        _responseDto.DisplayMessage = $"Error al Intentar Crear la Tarjeta de Crédito con la Cuenta de Correo {email} la Misma no Existe.";

                        return BadRequest(_responseDto);
                    }
                    else
                    {
                        logger.LogInformation("La Tarjeta de Crédito fue Editada Satisfactoriamente.");

                        _responseDto.Result = response;
                        _responseDto.DisplayMessage = "La Tarjeta de Crédito fue Editada Satisfactoriamente.";

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
        public async Task<ActionResult<CreditCard>> GetCreditCardById(int id)
        {
            var email = HttpContext.Session.GetString("userEmail");
            var token = HttpContext.Session.GetString("token");
            var isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            try
            {
                if (await _creditCardRepository.SuccessAuth(email, token, isAuthenticated))
                {
                    logger.LogInformation($"Intentando Obtener una Tarjeta de Crédito por el Id con la Cuenta de Correo {email}");

                    var response = await _creditCardRepository.GetCreditCardById(id, email);

                    if (response == null)
                    {
                        logger.LogError($"La Tarjeta de Crédito no Existe para la Cuenta de Correo {email}.");

                        _responseDto.IsSuccess = false;
                        _responseDto.DisplayMessage = $"La Tarjeta de Crédito no Existe para la Cuenta de Correo {email}.";

                        return BadRequest(_responseDto);
                    }
                    else
                    {
                        logger.LogInformation("Los Datos de la Tarjeta de Crédito fueron Obtenidos Satisfactoriamente.");

                        _responseDto.Result = response;
                        _responseDto.DisplayMessage = "Los Datos de la Tarjeta de Crédito fueron Obtenidos Satisfactoriamente.";

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
        public async Task<ActionResult<CreditCard>> DelteCreditCard(CreditCardDeleteDto creditCardDeleteDto)
        {
            //var email = HttpContext.Session.GetString("userEmail");
            //var token = HttpContext.Session.GetString("token");
            //var isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            try
            {
                logger.LogInformation($"Intentando Eliminar una Tarjeta de Crédito por el Id con la Cuenta de Correo {creditCardDeleteDto.Email}");

                var response = await _creditCardRepository.DeleteCreditCard(creditCardDeleteDto.Email, creditCardDeleteDto.Id);

                if (response == "NoCard")
                {
                    logger.LogError($"Error al Intentar Eliminar la Tarjeta de Crédito con la Cuenta de Correo {creditCardDeleteDto.Email} la Misma no Existe.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"Error al Intentar Eliminar la Tarjeta de Crédito con la Cuenta de Correo {creditCardDeleteDto.Email} la Misma no Existe.";

                    return BadRequest(_responseDto);
                }
                else
                {
                    logger.LogInformation("Los Datos de la Tarjeta de Crédito Fueron Eliminados Satisfactoriamente.");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = "Los Datos de la Tarjeta de Crédito Fueron Eliminados Satisfactoriamente.";

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

        [HttpPost("SetCardMain")]
        public async Task<ActionResult<CreditCard>> SetCreditMainCard(CreditMainCardDto creditMainCardDto)
        {
            //var email = HttpContext.Session.GetString("userEmail");
            //var token = HttpContext.Session.GetString("token");
            //var isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            try
            {
                logger.LogInformation($"Intentando Establecer una Tarjeta de Crédito como Principal con la Cuenta de Correo {creditMainCardDto.Email}");

                var response = await _creditCardRepository.PostMainCard(creditMainCardDto.Id, creditMainCardDto.Email);

                if (response == "NoCard")
                {
                    logger.LogError($"Error al Intentar Establecer la Tarjeta de Crédito como Principal con la Cuenta de Correo {creditMainCardDto.Email} ya Existe una Igual.");

                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = $"Error al Intentar Establecer la Tarjeta de Crédito como Principal con la Cuenta de Correo {creditMainCardDto.Email} ya Existe una Igual.";

                    return BadRequest(_responseDto);
                }
                else
                {
                    logger.LogInformation("La Tarjeta de Crédito fue Establecida com Principal Satisfactoriamente.");

                    _responseDto.Result = response;
                    _responseDto.DisplayMessage = "La Tarjeta de Crédito fue Establecida com Principal Satisfactoriamente.";

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
    }
}
