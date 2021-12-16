using MediatR;
using Microsoft.EntityFrameworkCore;
using Data.Model;
using Data.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Commands.Execute
{
    public class GetDefaultCardIdAsyncCommand : IRequest<long>
    {
        public long UserId { get; set; }

        public class GetDefaultCardIdAsyncCommandHandler : IRequestHandler<GetDefaultCardIdAsyncCommand, long>
        {
            private readonly IRepository _db;

            public GetDefaultCardIdAsyncCommandHandler(IRepository db)
            {
                _db = db;
            }

            public async Task<long> Handle(GetDefaultCardIdAsyncCommand findDefaultCard, CancellationToken cancellationToken)
            {
                var card = await _db.GetAll<CardModel>().Where(c => c.UserId == findDefaultCard.UserId && c.IsDefault).FirstOrDefaultAsync(cancellationToken: cancellationToken);
                return card != null ? card.Id : -1;
            }

        }
    }
}
