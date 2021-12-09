using MediatR;
using Middleware.Data.Model;
using Middleware.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Middleware.Data.Commands.Query
{
    public class GetTransactionByIdAsyncCommand : IRequest<Transaction>
    {
        public long Id { get; set; }

        public class GetTransactionByIdAsyncCommandHandler : IRequestHandler<GetTransactionByIdAsyncCommand, Transaction>
        {
            private readonly IRepository _db;

            public GetTransactionByIdAsyncCommandHandler(IRepository db)
            {
                _db = db;
            }

            public async Task<Transaction> Handle(GetTransactionByIdAsyncCommand t, CancellationToken cancellationToken) => await _db.GetAsync<Transaction>(t.Id);
        }
    }
}
