using API.Data;
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

namespace API.Repository.CreditCards
{
    public class CreditCardRepository : ICreditCardRepository
    {
        private readonly ApplicationDBContext _db;
        private IMapper _mapper;
        private readonly ILogger<CreditCardRepository> _logger;
        private readonly UserManager<GeneralDataUser> _userManager;

        public CreditCardRepository(ApplicationDBContext db, IMapper mapper, ILogger<CreditCardRepository> logger, UserManager<GeneralDataUser> userManager)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<string> CreateCreditCard(CreditCardDto creditCardDto)
        {
            CreditCard creditCard = _mapper.Map<CreditCardDto, CreditCard>(creditCardDto);

            var user = await _userManager.FindByEmailAsync(creditCardDto.Email);
            List<CreditCard> list = await _db.CreditCards.Where(p => p.GeneralDataUserId == user.Id && p.IsActive == true).ToListAsync();
            var creditCardFind = _db.CreditCards.FirstOrDefault(c => c.CardNumber == creditCard.CardNumber && c.GeneralDataUserId == user.Id);

            if (list.Count == 0 && creditCardFind == null && creditCard.Id <= 0)
            {
                _logger.LogInformation("Ejecutando la funcionalidad Adicionar Tarjeta de Crédito");

                creditCard.GeneralDataUserId = user.Id;
                creditCard.IsDefault = true;

                await _db.CreditCards.AddAsync(creditCard);

                await _db.SaveChangesAsync();

                //if (creditCardDto.IsDefault)
                //{
                //    await PostMainCard(creditCard.Id, email);
                //}

                return "Ok";
            }

            if (list.Count > 0 && creditCardFind == null && creditCard.Id <= 0)
            {
                _logger.LogInformation("Ejecutando la funcionalidad Adicionar Tarjeta de Crédito");

                creditCard.GeneralDataUserId = user.Id;
                creditCard.IsDefault = false;

                await _db.CreditCards.AddAsync(creditCard);

                await _db.SaveChangesAsync();

                //if (creditCardDto.IsDefault)
                //{
                //    await PostMainCard(creditCard.Id, email);
                //}

                return "Ok";
            }
            else
            {
                _logger.LogError("La Tarjeta de Crédito ya Existe");

                return "ExistCard";
            }

        }

        public async Task<string> UpdateCreditCard(string email, CreditCardUpdateDto creditCardUpdateDto)
        {

            var user = await _userManager.FindByEmailAsync(email);

            var IsRepeatRow = _db.CreditCards
                            .FirstOrDefault(
                                   c => c.CardNumber == creditCardUpdateDto.CardNumber
                                   && c.Id != creditCardUpdateDto.Id && c.GeneralDataUserId == user.Id && c.IsActive == true);

            var card = _db.CreditCards
                            .FirstOrDefault(
                                   c => c.CardNumber == creditCardUpdateDto.CardNumber && c.Id == creditCardUpdateDto.Id && c.GeneralDataUserId == user.Id && c.IsActive == true);

            if (IsRepeatRow != null)
            {
                return "IsRepeat";
            }

            if (card == null)
            {
                return "NoExist";
            }

            if (card.Id > 0)
            {
                _logger.LogInformation("Ejecutando la funcionalidad Actualizar Tarjeta de Crédito");

                card.CardNumber = creditCardUpdateDto.CardNumber;
                card.ExpiredDate = creditCardUpdateDto.ExpiredDate;
                card.UserName = creditCardUpdateDto.UserName;
                card.IsDefault = creditCardUpdateDto.IsDefault;

                _db.CreditCards.Update(card);

                await _db.SaveChangesAsync();

                if (creditCardUpdateDto.IsDefault)
                {
                    await PostMainCard(card.Id, email);
                }

                return "Ok";
            }
            else
            {
                _logger.LogError("La Tarjeta de Crédito no Existe");

                return "NoCard";
            }

        }

        public async Task<string> DeleteCreditCard(string email, int id)
        {
            _logger.LogInformation("Ejecutando la funcionalidad Eliminar Tarjeta de Crédito por ID");

            var user = await _userManager.FindByEmailAsync(email);

            CreditCard creditCard = await _db.CreditCards.Where(p => p.Id == id && p.GeneralDataUserId == user.Id && p.IsActive == true).FirstOrDefaultAsync();

            if (creditCard == null)
            {
                return "NoCard";
            }
            else
            {
                _db.CreditCards.Remove(creditCard);

                //creditCard.IsActive = false;

                //_db.CreditCards.Update(creditCard);

                await _db.SaveChangesAsync();

                return "Ok";
            }
        }

        public async Task<List<CreditCardUpdateDto>> GetCreditCards(string email)
        {
            _logger.LogInformation("Ejecutando la funcionalidad Obtener Lista de Tarjetas de Crédito");

            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                List<CreditCard> list = await _db.CreditCards.Where(p => p.GeneralDataUserId == user.Id && p.IsActive == true).ToListAsync();

                return _mapper.Map<List<CreditCardUpdateDto>>(list);
            }
            else
            {
                return null;
            }
        }

        public async Task<CreditCardDto> GetCreditCardById(int id, string email)
        {
            _logger.LogInformation("Ejecutando la funcionalidad Obtener Tarjeta de Crédito por ID");

            var user = await _userManager.FindByEmailAsync(email);

            CreditCard creditCard = await _db.CreditCards.Where(p => p.Id == id && p.GeneralDataUserId == user.Id && p.IsActive == true).FirstOrDefaultAsync();

            if (creditCard != null)
            {
                return _mapper.Map<CreditCardDto>(creditCard);
            }
            else
            {
                return null;
            }
        }

        public async Task<CreditCardDto> GetCreditCardByNumber(string cardNumber, string email)
        {
            _logger.LogInformation("Ejecutando la funcionalidad Obtener Tarjeta de Crédito por Número");

            if (email != "")
            {
                var user = await _userManager.FindByEmailAsync(email);

                CreditCard creditCard = await _db.CreditCards.Where(p => p.CardNumber == cardNumber && p.GeneralDataUserId == user.Id).FirstOrDefaultAsync();

                if (creditCard != null)
                {
                    return _mapper.Map<CreditCardDto>(creditCard);
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

        public async Task<string> DeleteCreditCardByNumber(string email, string cardNumber)
        {
            _logger.LogInformation("Ejecutando la funcionalidad Eliminar Tarjeta de Crédito por Número");

            if (email != "")
            {
                var user = await _userManager.FindByEmailAsync(email);

                CreditCard creditCard = await _db.CreditCards.Where(p => p.CardNumber == cardNumber && p.GeneralDataUserId == user.Id).FirstOrDefaultAsync();

                if (creditCard == null)
                {
                    return "NoCard";
                }
                else
                {
                    _db.CreditCards.Remove(creditCard);

                    await _db.SaveChangesAsync();

                    return "Ok";
                }
            }

            return "NotEmpty";
        }

        public async Task<string> PostMainCard(int id, string email)
        {
            _logger.LogInformation($"Ejecutando la Funcionalidad Establecer Tarjeta de Crédito como Principal para el Usuario con cuenta de correo {email}");

            var user = await _userManager.FindByEmailAsync(email);
            var creditCard = await _db.CreditCards.FindAsync(id);

            List<CreditCard> list = await _db.CreditCards.Where(p => p.GeneralDataUserId == user.Id && p.IsActive == true).ToListAsync();

            if (creditCard != null)
            {
                creditCard.IsDefault = true;

                _db.Update(creditCard);

                await _db.SaveChangesAsync();

                await FalseAllAsync(list, id, user.Id);

                return "Ok";
            }
            else
            {
                _logger.LogError("La Tarjeta de Crédito no Existe");

                return "NoCard";
            }
        }

        private async Task FalseAllAsync(List<CreditCard> cards, int id, string userId) 
        {
            if (cards.Count() > 0)
            {
                foreach (var card in cards)
                {
                    if (card.IsDefault && card.Id != id)
                    {
                        card.IsDefault = false;

                        _db.Update(card);

                        await _db.SaveChangesAsync();
                    }
                }
            }
            
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
    }
}
