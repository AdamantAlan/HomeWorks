using MediatR;
using Middleware.Data.Model;
using Middleware.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Middleware.Data.Commands.Execute
{
    /// <summary>
    /// Change CardHolder for user cards.
    /// </summary>
    public class ChangeCardHolderNameCardsAsyncCommand : IRequest<bool>
    {
        public IEnumerable<Card> Cards { get; set; }
        public string CardHolderName { get; set; }

        public class ChangeCardHolderNameCardsAsyncCommandHandler : IRequestHandler<ChangeCardHolderNameCardsAsyncCommand, bool>
        {
            private readonly IRepository _db;

            public ChangeCardHolderNameCardsAsyncCommandHandler(IRepository db)
            {
                _db = db;
            }
            public async Task<bool> Handle(ChangeCardHolderNameCardsAsyncCommand change, CancellationToken cancellationToken)
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
