using MediatR;
using Microsoft.EntityFrameworkCore;
using Data.Model;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Commands.Query
{
    public class GetAllTransactionsAsyncCommand : IRequest<IEnumerable<TransactionModel>>
    {
        public long UserId { get; set; }

        public class GetAllTransactionsAsyncCommandHandler : IRequestHandler<GetAllTransactionsAsyncCommand, IEnumerable<TransactionModel>>
        {
            private readonly IRepository _db;

            public GetAllTransactionsAsyncCommandHandler(IRepository db)
            {
                _db = db;
            }

            public async Task<IEnumerable<TransactionModel>> Handle(GetAllTransactionsAsyncCommand getTransaction, CancellationToken cancellationToken) =>

                await _db.GetAll<TransactionModel>().Where(c => c.UserId == getTransaction.UserId).ToListAsync();

        }
    }
}
