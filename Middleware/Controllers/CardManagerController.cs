using Microsoft.AspNetCore.Mvc;
using Middleware.Data;
using Middleware.Dto;
using Middleware.Extensions;
using Middleware.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Middleware.Controllers
{
    /// <summary>
    /// Controller for work with user cards.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ResultApi<string>), 401)]
    [ProducesResponseType(typeof(ResultApi<string>), 500)]
    [ProducesResponseType(typeof(ResultApi<string>), 404)]
    public class CardManagerController : ControllerBase
    {
        private readonly ICardManager _manager;

        public CardManagerController(ICardManager cardManager)
        {
            _manager = cardManager;
        }

        [HttpGet("test")]
        public ActionResult<ResultApi<string>> Test() => new ResultApi<string> { Result = "Success!" };

        /// <summary>
        /// Add new user card.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="card">New user card</param>
        /// <response code="200">Card added</response>
        /// <response code="500">Card dont added, server error</response>
        /// <response code="404">Non used</response>
        /// <returns>Result work server</returns>
        [ProducesResponseType(typeof(ResultApi<CardReadDto>), 200)]
        [HttpPost("{userId:long}/[action]")]
        public ActionResult<ResultApi<CardReadDto>> AddCard(long userId, Card card)
        {
            card.userId = userId;
            var cardDto = _manager.SetCard(card);
            return new ResultApi<CardReadDto> { Result = cardDto, ErrorCode = 0, ErrorMessage = null };
        }

        /// <summary>
        /// Delete user card.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="pan">Number pan card</param>
        /// <response code="200">Card added</response>
        /// <response code="404">Not found user or card</response>
        /// <response code="500">Card dont added, server error</response>
        /// <returns>Result work server</returns>
        [ProducesResponseType(typeof(ResultApi<CardReadDto>), 200)]
        [HttpDelete("{userId:long}/[action]")]
        public ActionResult<ResultApi<CardReadDto>> DeleteCard(long userId, [FromBody] string pan)
        {
            if (!_manager.UserExist(userId))
                return NotFound(new ResultApi<string> { Result = userId.ToString(), ErrorCode = 800, ErrorMessage = "User not found." });

            if (!_manager.GetCards(userId).Any(c => c.Pan == pan))
                return NotFound(new ResultApi<string> { Result = userId.ToString(), ErrorCode = 799, ErrorMessage = $"User has no current card with pan {pan.GetHiddenPan()}" });

            var cardDto = _manager.DeleteCard(userId, pan);
            return new ResultApi<CardReadDto> { Result = cardDto, ErrorCode = 0, ErrorMessage = null };
        }

        /// <summary>
        /// Get user cards
        /// </summary>
        /// <param name="userId">User id</param>
        /// <response code="200">Cards getted</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Cards dont added, server error</response>
        /// <returns>Result work server</returns>
        [ProducesResponseType(typeof(ResultApi<IEnumerable<CardReadDto>>), 200)]
        [HttpGet("{userId:long}/[action]")]
        public ActionResult<ResultApi<IEnumerable<CardReadDto>>> GetCards(long userId)
        {
            if (!_manager.UserExist(userId))
                return NotFound(new ResultApi<string> { Result = userId.ToString(), ErrorCode = 800, ErrorMessage = "User not found." });

            var cards = _manager.GetCards(userId);

            return new ResultApi<IEnumerable<CardReadDto>> { Result = cards, ErrorCode = 0, ErrorMessage = null };
        }

        /// <summary>
        /// Get user one card
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="pan">Number pan card</param>
        /// <response code="200">Card getted</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Card dont added, server error</response>
        /// <returns>Result work server</returns>
        [ProducesResponseType(typeof(ResultApi<CardReadDto>), 200)]
        [HttpPost("{userId:long}/[action]")]
        public ActionResult<ResultApi<CardReadDto>> GetCard([FromRoute] long userId, [FromBody] string pan)
        {
            if (!_manager.UserExist(userId))
                return NotFound(new ResultApi<string> { Result = userId.ToString(), ErrorCode = 800, ErrorMessage = "User not found." });

            var card = _manager.GetCards(userId).FirstOrDefault(c => c.Pan == pan);

            if (card != null)
                return new ResultApi<CardReadDto> { Result = card, ErrorCode = 0, ErrorMessage = null };
            else
                return NotFound(new ResultApi<string> { Result = userId.ToString(), ErrorCode = 799, ErrorMessage = $"User has no current card with pan {pan}" });
        }

        /// <summary>
        /// Change cardHolder on user cards
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cardHolderName">New nameholder on cards</param>
        /// <response code="200">Card changed</response>
        /// <response code="404">User not found</response>
        /// <response code="400">Wrong cardholder</response>
        /// <response code="500">Server error</response>
        /// <returns>Result work server</returns>
        [ProducesResponseType(typeof(ResultApi<IEnumerable<CardReadDto>>), 200)]
        [ProducesResponseType(typeof(ResultApi<string>), 400)]
        [HttpPatch("{userId:long}/[action]")]
        public ActionResult<ResultApi<IEnumerable<CardReadDto>>> ChangeCardHolder([FromRoute] long userId, [FromBody] string cardHolderName)
        {
            if (!_manager.UserExist(userId))
                return NotFound(new ResultApi<string> { Result = userId.ToString(), ErrorCode = 800, ErrorMessage = "User not found." });

            if (long.TryParse(cardHolderName, out long c))
                return BadRequest(new ResultApi<string> { Result = userId.ToString(), ErrorCode = 801, ErrorMessage = "Wrong cardholder." });

            var cards = _manager.ChangeCardHolder(userId, cardHolderName);
            return new ResultApi<IEnumerable<CardReadDto>> { Result = cards, ErrorCode = 0, ErrorMessage = null };
        }
    }
}
