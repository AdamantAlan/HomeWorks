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
    public class GetCardForUserIdAndPanCommand : IRequest<Card>
    {
        public string Pan { get; set; }
        public long UserId { get; set; }

        public class GetCardForUserIdAndPanCommandHandler : IRequestHandler<GetCardForUserIdAndPanCommand, Card>
        {
            private readonly IRepository _db;
            public GetCardForUserIdAndPanCommandHandler(IRepository db)
            {
                _db = db;
            }

            public async Task<Card> Handle(GetCardForUserIdAndPanCommand get, CancellationToken cancellationToken)
            {
                return await _db.GetAll<Card>().FirstOrDefaultAsync(c => c.UserId == get.UserId && c.Pan == get.Pan);
            }
        }
    }
}
