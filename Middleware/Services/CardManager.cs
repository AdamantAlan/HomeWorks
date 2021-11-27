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

        public CardReadDto SetCard(Card card)
        {
            Cards.Add(card);
            return _map.Map<CardReadDto>(card);
        }

        public bool UserExist(long id) => Cards.Any(c => c.UserId == id);

        public IEnumerable<CardReadDto> ChangeCardHolder(long id, string newName)
        {
            foreach (var c in Cards)
            {
                if (c.UserId == id)
                    c.Name = newName;
            }

            return GetCards(id);
        }

        public CardReadDto DeleteCard(long userId, string pan)
        {
            var currentCard = Cards.FirstOrDefault(c => c.UserId == userId && c.Pan == pan);
            Cards.Remove(currentCard);

            return _map.Map<CardReadDto>(currentCard);
        }
    }
}
