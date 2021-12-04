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
    public class ChangeCardHolderNameCardsCommand : IRequest<bool>
    {
        public IEnumerable<Card> Cards { get; set; }
        public string CardHolderName { get; set; }

        public class ChangeCardHolderNameCardsCommandHandler : IRequestHandler<ChangeCardHolderNameCardsCommand, bool>
        {
            private readonly IRepository _db;

            public ChangeCardHolderNameCardsCommandHandler(IRepository db)
            {
                _db = db;
            }
            public async Task<bool> Handle(ChangeCardHolderNameCardsCommand change, CancellationToken cancellationToken)
            {
                foreach (Card card in change.Cards)
                {
                    card.Name = change.CardHolderName;
                }

                return await _db.SaveChangeAsync();
            }
        }
    }
}
