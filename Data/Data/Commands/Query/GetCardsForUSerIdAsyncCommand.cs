using MediatR;
using Microsoft.EntityFrameworkCore;
using Data.Model;
using Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Commands.Query
{
    /// <summary>
    /// Get user cards for user id.
    /// </summary>
    public class GetCardsForUSerIdAsyncCommand : IRequest<IEnumerable<CardModel>>
    {
        public long UserId { get; set; }

        public class GetCardsForUSerIdAsyncCommandHandler : IRequestHandler<GetCardsForUSerIdAsyncCommand, IEnumerable<CardModel>>
        {
            private readonly IRepository _db;

            public GetCardsForUSerIdAsyncCommandHandler(IRepository db)
            {
                _db = db;
            }

            public async Task<IEnumerable<CardModel>> Handle(GetCardsForUSerIdAsyncCommand getCards, CancellationToken cancellationToken)
            {
                return await _db.GetAll<CardModel>().Where(c => c.UserId == getCards.UserId).ToListAsync();
            }
        }
    }
}
