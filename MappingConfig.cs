using API.Models;
using API.Models.Dto;
using AutoMapper;

namespace API
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps() 
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CreditCardUpdateDto, CreditCard>();
                config.CreateMap<CreditCard, CreditCardUpdateDto>();

                config.CreateMap<CreditCardDto, CreditCard>();
                config.CreateMap<CreditCard, CreditCardDto>();

                config.CreateMap<PropertyDto, Property>();
                config.CreateMap<Property, PropertyDto>();

                config.CreateMap<PropertyUpdateDto, Property>();
                config.CreateMap<Property, PropertyUpdateDto>();

                config.CreateMap<C_MEDIDORDto, C_MEDIDOR>();
                config.CreateMap<C_MEDIDOR, C_MEDIDORDto>();
            });

            return mappingConfig;

        }
    }
}
