using AutoMapper;
using Data.Data.MessageMQ;
using Data.Dto;
using Data.Model;

namespace Transaction.Profiles
{
    /// <summary>
    /// Opions mapper for card.
    /// <see cref="CardModel"/>
    /// <see cref="CardReadDto"/>
    /// <see cref="CardWriteDto"/>
    /// Opions mapper for transaction.
    /// <see cref="TransactionModel"/>
    /// <see cref="TransactionReadDto"/>
    /// <see cref="TransactionWriteDto"/>
    /// <see cref="NewCardMessage"/>
    /// </summary>
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<CardModel, CardReadDto>();
            CreateMap<CardWriteDto, CardModel>();
            CreateMap<CardWriteDto, NewCardMessage>();
            CreateMap<TransactionModel, TransactionReadDto>();
            CreateMap<TransactionWriteDto, TransactionModel>();
        }
    }
}
