using MediatR;
using Microsoft.EntityFrameworkCore;
using Data.Model;
using Data.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Commands.Query
{
    /// <summary>
    /// Get user card for user id and number pan.
    /// </summary>
    public class GetCardForUserIdAndPanAsyncCommand : IRequest<CardModel>
    {
        public string Pan { get; set; }
        public long UserId { get; set; }

        public class GetCardForUserIdAndPanAsyncCommandHandler : IRequestHandler<GetCardForUserIdAndPanAsyncCommand, CardModel>
        {
            private readonly IRepository _db;
            public GetCardForUserIdAndPanAsyncCommandHandler(IRepository db)
            {
                _db = db;
            }

            public async Task<CardModel> Handle(GetCardForUserIdAndPanAsyncCommand get, CancellationToken cancellationToken)
            {
                return await _db.GetAll<CardModel>().FirstOrDefaultAsync(c => c.UserId == get.UserId && c.Pan == get.Pan);
            }
        }
    }
}
