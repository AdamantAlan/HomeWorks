using MediatR;
using Middleware.Data.Model;
using Middleware.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Middleware.Data.Commands.Execute
{
    /// <summary>
    /// Delete user card.
    /// </summary>
    public class DeleteCardAsyncCommand : IRequest<bool>
    {
        public Card Card { get; set; }

        public class DeleteCardAsyncCommandHandler : IRequestHandler<DeleteCardAsyncCommand, bool>
        {
            private readonly IRepository _db;

            public DeleteCardAsyncCommandHandler(IRepository db)
            {
                _db = db;
            }

            public async Task<bool> Handle(DeleteCardAsyncCommand delete, CancellationToken cancellationToken)
            {
                return await _db.DeleteAsync(delete.Card);
            }
        }
    }
}
