using MediatR;
using Microsoft.EntityFrameworkCore;
using Middleware.Data.Model;
using Middleware.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Middleware.Data.Commands.Query
{
    public class GetAllTransactionsAsyncCommand : IRequest<IEnumerable<Transaction>>
    {
        public long UserId { get; set; }

        public class GetAllTransactionsAsyncCommandHandler : IRequestHandler<GetAllTransactionsAsyncCommand, IEnumerable<Transaction>>
        {
            private readonly IRepository _db;

            public GetAllTransactionsAsyncCommandHandler(IRepository db)
            {
                _db = db;
            }

            public async Task<IEnumerable<Transaction>> Handle(GetAllTransactionsAsyncCommand getTransaction, CancellationToken cancellationToken) =>

                await _db.GetAll<Transaction>().Where(c => c.UserId == getTransaction.UserId).ToListAsync();

        }
    }
}
