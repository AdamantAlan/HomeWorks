using MediatR;
using Data.Model;
using Data.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Commands.Execute
{
    /// <summary>
    /// Add user card.
    /// </summary>
    public class CreateCardAsyncCommand : IRequest<long>
    {
        public CardModel Card { get; set; }

        public class CreateCardAsyncCommandHandler : IRequestHandler<CreateCardAsyncCommand, long>
        {
            private readonly IRepository _db;

            public CreateCardAsyncCommandHandler(IRepository db)
            {
                _db = db;
            }

            public async Task<long> Handle(CreateCardAsyncCommand create, CancellationToken cancellationToken)
            {
                return await _db.CreateAsync(create.Card);
            }
        }
    }
}
