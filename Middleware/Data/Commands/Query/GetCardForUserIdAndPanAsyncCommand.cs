using MediatR;
using Microsoft.EntityFrameworkCore;
using Middleware.Data.Model;
using Middleware.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Middleware.Data.Commands.Query
{
    /// <summary>
    /// Get user card for user id and number pan.
    /// </summary>
    public class GetCardForUserIdAndPanAsyncCommand : IRequest<Card>
    {
        public string Pan { get; set; }
        public long UserId { get; set; }

        public class GetCardForUserIdAndPanAsyncCommandHandler : IRequestHandler<GetCardForUserIdAndPanAsyncCommand, Card>
        {
            private readonly IRepository _db;
            public GetCardForUserIdAndPanAsyncCommandHandler(IRepository db)
            {
                _db = db;
            }

            public async Task<Card> Handle(GetCardForUserIdAndPanAsyncCommand get, CancellationToken cancellationToken)
            {
                return await _db.GetAll<Card>().FirstOrDefaultAsync(c => c.UserId == get.UserId && c.Pan == get.Pan);
            }
        }
    }
}
