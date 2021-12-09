using AutoMapper;
using Middleware.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Middleware.Profiles
{
    /// <summary>
    /// Opions mapper for transaction.
    /// <see cref="Transaction"/>
    /// <see cref="TransactionReadDto"/>
    /// <see cref="TransactionWriteDto"/>
    /// </summary>
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, TransactionReadDto>();
            CreateMap<TransactionWriteDto, Transaction>();
        }
    }
}
