using MediatR;
using Middleware.Data.Model;
using Middleware.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Middleware.Data.Commands.Execute
{
    /// <summary>
    /// Add user card.
    /// </summary>
    public class CreateCardAsyncCommand : IRequest<long>
    {
        public Card Card { get; set; }

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
