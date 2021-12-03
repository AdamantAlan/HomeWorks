using Middleware.Data;
using Middleware.Dto;
using System.Collections.Generic;

namespace Middleware.Interfaces
{
    /// <summary>
    /// Contract for work with user cards.
    /// </summary>
    public interface ICardManager
    {
        /// <summary>
        /// database for user cards.
        /// </summary>
        List<Card> Cards { get; set; }

        /// <summary>
        /// Write card in database.
        /// </summary>
        /// <param name="card">user card.</param>
        CardReadDto SetCard(CardWriteDto card);

        /// <summary>
        /// Check exist user.
        /// </summary>
        /// <returns>true - user exist;false - user not exist</returns>
        bool UserExist(long id);

        /// <summary>
        /// Get all cards of user.
        /// </summary>
        /// <param name="id">user id</param>
        IEnumerable<CardReadDto> GetCards(long id);

        /// <summary>
        /// Change cardholder for user cards.
        /// </summary>
        IEnumerable<CardReadDto> ChangeCardHolder(long id, string newName);

        /// <summary>
        ///Delete user card.
        /// </summary>
        CardReadDto DeleteCard(long userId, string pan);
    }
}
