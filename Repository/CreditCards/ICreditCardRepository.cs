
using API.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Repository.CreditCards
{
    public interface ICreditCardRepository
    {
        Task<List<CreditCardUpdateDto>> GetCreditCards(string email);

        Task<CreditCardDto> GetCreditCardById(int id, string email);

        Task<CreditCardDto> GetCreditCardByNumber(string cardNumber, string email);

        Task<string> CreateCreditCard(CreditCardDto creditCardDto);

        Task<string> UpdateCreditCard(string email, CreditCardUpdateDto creditCardUpdateDto);

        Task<string> DeleteCreditCard(string email, int id);

        Task<string> DeleteCreditCardByNumber(string email, string cardNumber);

        Task<string> PostMainCard(int id, string email);

        Task<bool> SuccessAuth(string email, string token, string isAuthenticated);
    }
}
