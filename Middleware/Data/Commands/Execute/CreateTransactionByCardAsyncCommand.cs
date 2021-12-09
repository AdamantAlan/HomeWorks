using MediatR;
using Middleware.Data.Model;
using Middleware.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Middleware.Data.Commands.Execute
{
    public class CreateTransactionByCardAsyncCommand : IRequest<long>
    {
        public long UserId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public long CardId { get; set; }

        public class CreateTransactionByCardAsyncCommandHandeler : IRequestHandler<CreateTransactionByCardAsyncCommand, long>
        {
            private readonly IRepository _db;

            public CreateTransactionByCardAsyncCommandHandeler(IRepository db)
            {
                _db = db;
            }

            public async Task<long> Handle(CreateTransactionByCardAsyncCommand t, CancellationToken cancellationToken)
            {
                var id = await _db.CreateAsync(new Transaction
                {
                    UserId = t.UserId,
                    DateOfTransaction = DateTime.Now,
                    Operation = t.TransactionType,
                    Amount = t.Amount,
                    CardId = t.CardId
                });

                await _db.SaveChangeAsync();

                return id;
            }
        }
    }
}
