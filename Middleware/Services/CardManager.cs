using AutoMapper;
using Middleware.Data;
using Middleware.Dto;
using Middleware.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Middleware.Services
{
    public class CardManager : ICardManager
    {
        public List<Card> Cards { get; set; } = new List<Card>();

        private readonly IMapper _map;

        public CardManager(IMapper map)
        {
            _map = map;
        }

        public IEnumerable<CardReadDto> GetCards(long id)
        {
            var cards = Cards.Where(c => c.UserId == id);
            var result = _map.Map<IEnumerable<CardReadDto>>(cards);
            return result;
        }

        public CardWriteDto SetCard(Card card)
        {
            Cards.Add(card);
            return _map.Map<CardWriteDto>(card);
        }
    }
}
