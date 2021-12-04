using MediatR;
using Middleware.Data.Model;
using Middleware.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Middleware.Data.Commands.Execute
{
    public class CreateCardCommand : IRequest<long>
    {
        public Card Card { get; set; }

        public class CreateCardCommandHandler : IRequestHandler<CreateCardCommand, long>
        {
            private readonly IRepository _db;

            public CreateCardCommandHandler(IRepository db)
            {
                _db = db;
            }

            public async Task<long> Handle(CreateCardCommand create, CancellationToken cancellationToken)
            {
                return await _db.CreateAsync(create.Card);
            }
        }
    }
}
