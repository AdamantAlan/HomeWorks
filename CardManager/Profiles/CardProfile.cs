using AutoMapper;
using Data.Dto;
using Data.Model;
using Data.Data.MessageMQ;

namespace CardManager.Profiles
{
    /// <summary>
    /// Opions mapper for card.
    /// <see cref="CardModel"/>
    /// <see cref="CardReadDto"/>
    /// <see cref="CardWriteDto"/>
    /// <see cref= "NewCardMessage" />    
    /// </summary>
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<CardModel, CardReadDto>();
            CreateMap<CardWriteDto, CardModel>();
            CreateMap<NewCardMessage, CardModel>();
        }
    }
}
