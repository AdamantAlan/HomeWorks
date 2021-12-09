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
    public class CheckCardExistAsyncCommand : IRequest<bool>
    {
        public long CardId { get; set; }

        public class CheckCardExistAsyncCommandHandler : IRequestHandler<CheckCardExistAsyncCommand, bool>
        {
            private readonly IRepository _db;

            public CheckCardExistAsyncCommandHandler(IRepository db)
            {
                _db = db;
            }

            public async Task<bool> Handle(CheckCardExistAsyncCommand check, CancellationToken cancellationToken) => await _db.EntityExist<Card>(check.CardId);
        }
    }
}
