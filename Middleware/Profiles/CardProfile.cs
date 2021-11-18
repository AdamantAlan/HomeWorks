using AutoMapper;
using Middleware.Data;
using Middleware.Dto;

namespace Middleware.Profiles
{
    /// <summary>
    /// Настройка маппера для модели Card и Dto
    /// <see cref="Card"/>
    /// <see cref="CardReadDto"/>
    /// <see cref="CardWriteDto"/>
    /// </summary>
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<Card, CardReadDto>();
            CreateMap<Card, CardWriteDto>();
        }
    }
}
