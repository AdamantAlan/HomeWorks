using MediatR;
using Data.Model;
using Data.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Commands.Execute
{
    /// <summary>
    /// Change CardHolder for user cards.
    /// </summary>
    public class ChangeCardHolderNameCardsAsyncCommand : IRequest<bool>
    {
        public IEnumerable<CardModel> Cards { get; set; }
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
                foreach (CardModel card in change.Cards)
                {
                    card.Name = change.CardHolderName;
                }

                return await _db.SaveChangeAsync();
            }
        }
    }
}
