using MediatR;
using Microsoft.EntityFrameworkCore;
using Middleware.Data.Model;
using Middleware.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Middleware.Data.Commands.Query
{
    /// <summary>
    /// Get user cards for user id.
    /// </summary>
    public class GetCardsForUSerIdAsyncCommand : IRequest<IEnumerable<Card>>
    {
        public long UserId { get; set; }

        public class GetCardsForUSerIdAsyncCommandHandler : IRequestHandler<GetCardsForUSerIdAsyncCommand, IEnumerable<Card>>
        {
            private readonly IRepository _db;

            public GetCardsForUSerIdAsyncCommandHandler(IRepository db)
            {
                _db = db;
            }

            public async Task<IEnumerable<Card>> Handle(GetCardsForUSerIdAsyncCommand getCards, CancellationToken cancellationToken)
            {
                return await _db.GetAll<Card>().Where(c => c.UserId == getCards.UserId).ToListAsync();
            }
        }
    }
}
