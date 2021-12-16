using AutoMapper;
using Data.Dto;
using Data.Model;
using Data.Dto;

namespace CardManager.Profiles
{
    /// <summary>
    /// Opions mapper for card.
    /// <see cref="CardModel"/>
    /// <see cref="CardReadDto"/>
    /// <see cref="CardWriteDto"/>
    /// </summary>
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<CardModel, CardReadDto>();
            CreateMap<CardWriteDto, CardModel>();
        }
    }
}
