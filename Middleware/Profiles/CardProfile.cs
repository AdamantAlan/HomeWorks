using AutoMapper;
using Middleware.Data.Dto;
using Middleware.Data.Model;
using Middleware.Dto;

namespace Middleware.Profiles
{
    /// <summary>
    /// Opions mapper for card.
    /// <see cref="Card"/>
    /// <see cref="CardReadDto"/>
    /// <see cref="CardWriteDto"/>
    /// </summary>
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<Card, CardReadDto>();
            CreateMap<CardWriteDto, Card>();
            CreateMap<Transaction, TransactionReadDto>();
            CreateMap<TransactionWriteDto, Transaction>();
        }
    }
}
