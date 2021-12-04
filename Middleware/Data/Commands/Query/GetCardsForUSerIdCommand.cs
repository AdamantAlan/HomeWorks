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
    public class GetCardsForUSerIdCommand:IRequest<IEnumerable<Card>>
    {
        public long UserId { get; set; }

        public class GetCardsForUSerIdCommandHandler : IRequestHandler<GetCardsForUSerIdCommand, IEnumerable<Card>>
        {
            private readonly IRepository _db;

            public GetCardsForUSerIdCommandHandler(IRepository db)
            {
                _db = db;
            }

            public async Task<IEnumerable<Card>> Handle(GetCardsForUSerIdCommand getCards, CancellationToken cancellationToken)
            {
              return await _db.GetAll<Card>().Where(c => c.UserId == getCards.UserId).ToListAsync();
            }
        }
    }
}
