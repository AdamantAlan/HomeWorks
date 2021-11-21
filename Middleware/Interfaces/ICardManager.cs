using Middleware.Data;
using Middleware.Dto;
using System.Collections.Generic;

namespace Middleware.Interfaces
{
    /// <summary>
    /// Контракт для обработки карт пользователей.
    /// </summary>
    public interface ICardManager
    {
        // Можно было бы создать User с cards, поздно задумался.
        /// <summary>
        /// "БД" карт пользователей.
        /// </summary>
        List<Card> Cards { get; set; }

        /// <summary>
        /// Записать карту в "БД".
        /// </summary>
        /// <param name="card">Карта пользователя.</param>
        CardWriteDto SetCard(Card card);

        /// <summary>
        /// Выдать все карты пользователя.
        /// </summary>
        /// <param name="id">id пользователя в "БД".</param>
        /// <returns></returns>
        IEnumerable<CardReadDto> GetCards(long id);
    }
}
