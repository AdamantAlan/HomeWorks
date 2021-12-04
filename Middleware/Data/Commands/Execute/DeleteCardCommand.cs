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
    public class DeleteCardCommand : IRequest<bool>
    {
        public Card Card { get; set; }

        public class DeleteCardCommandHandler : IRequestHandler<DeleteCardCommand, bool>
        {
            private readonly IRepository _db;

            public DeleteCardCommandHandler(IRepository db)
            {
                _db = db;
            }

            public async Task<bool> Handle(DeleteCardCommand delete, CancellationToken cancellationToken)
            {
               return await _db.DeleteAsync(delete.Card);
            }
        }
    }
}
