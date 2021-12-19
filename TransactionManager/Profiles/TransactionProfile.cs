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
    /// <see cref="NewCardMessageV1"/>
    /// Opions mapper for transaction.
    /// <see cref="TransactionModel"/>
    /// <see cref="TransactionReadDto"/>
    /// <see cref="TransactionWriteDto"/>
    /// </summary>
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<CardModel, CardReadDto>();
            CreateMap<CardWriteDto, CardModel>();
            CreateMap<CardWriteDto, NewCardMessageV1>();
            CreateMap<CardWriteDto, NewCardMessageV2>();
            CreateMap<TransactionModel, TransactionReadDto>();
            CreateMap<TransactionWriteDto, TransactionModel>();
        }
    }
}
