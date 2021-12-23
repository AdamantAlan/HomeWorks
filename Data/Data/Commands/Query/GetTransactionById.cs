using MediatR;
using Data.Model;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Commands.Query
{
    public class GetTransactionByIdAsyncCommand : IRequest<TransactionModel>
    {
        public long Id { get; set; }

        public class GetTransactionByIdAsyncCommandHandler : IRequestHandler<GetTransactionByIdAsyncCommand, TransactionModel>
        {
            private readonly IRepository _db;

            public GetTransactionByIdAsyncCommandHandler(IRepository db)
            {
                _db = db;
            }

            public async Task<TransactionModel> Handle(GetTransactionByIdAsyncCommand t, CancellationToken cancellationToken) => await _db.GetAsync<TransactionModel>(t.Id);
        }
    }
}
