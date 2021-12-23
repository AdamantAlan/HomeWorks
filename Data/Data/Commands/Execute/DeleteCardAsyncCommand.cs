using MediatR;
using Data.Model;
using Data.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Commands.Execute
{
    /// <summary>
    /// Delete user card.
    /// </summary>
    public class DeleteCardAsyncCommand : IRequest<bool>
    {
        public CardModel Card { get; set; }

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
