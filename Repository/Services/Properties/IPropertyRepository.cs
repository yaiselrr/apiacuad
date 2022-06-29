using API.Models;
using API.Models.Dto;
using API.OtherModels.DB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Repository.Services.Properties
{
    public interface IPropertyRepository
    {
        Task<List<Property>> GetProperties(string email);

        Task<PropertyDto> GetPropertyById(string id, string email);

        Task<string> CreateProperty(PropertyDto propertyDto);

        Task<string> UpdateProperty(string email, PropertyUpdateDto propertyUpdateDto);

        Task<string> DeleteProperty(string email, string id);

        Task<string> TransferProperty(TransferPropertyDto model);

        Task<bool> SuccessAuth(string email, string token, string isAuthenticated);

        Task<string> AuthorizeUser(string email, string userId, string propertyId);

        Task<string> NoAuthorizeUser(string email, string userId, string propertyId);

        bool ValidateInsertAllAsync(int estructuraHidraulica, int derivadum, int toma);

        EstructuraHidraulica GetDataProperty(int estructuraHidraulica, int derivadum, int toma);

        string GetTypeService(int type);

        List<BalanceDto> GetBalance(int property);

        Task<string> DeletePropertyEnd(string id);

        Task<List<string>> GetPropertiesNoMain(string email);

        Task<List<Property>> GetPropertiesMain(string email);
    }
}
