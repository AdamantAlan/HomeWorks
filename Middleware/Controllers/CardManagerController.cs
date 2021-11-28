using Microsoft.AspNetCore.Mvc;
using Middleware.Data;
using Middleware.Interfaces;
using System;
using System.Linq;

namespace Middleware.Controllers
{
    /// <summary>
    /// Controller for work with user cards.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CardManagerController : ControllerBase
    {
        private readonly ICardManager _manager;

        public CardManagerController(ICardManager cardManager)
        {
            _manager = cardManager;
        }

        [HttpGet("test")]
        public ActionResult<string> Test()
        {
            return Ok(new ResultApi { Result = null });
        }

        /// <summary>
        /// Add new user card.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="card">New user card</param>
        /// <response code="200">Card added</response>
        /// <response code="500">Card dont added, server error</response>
        /// <returns>Result work server</returns>
        [ProducesResponseType(typeof(ResultApi), 200)]
        [ProducesResponseType(typeof(ResultApi), 500)]
        [HttpPost("{userId:long}/[action]")]
        public ActionResult<ResultApi> AddCard(long userId, Card card)
        {
            card.userId = userId;

            try
            {
                var cardDto = _manager.SetCard(card);

                return Ok(new ResultApi { Result = cardDto, ErrorCode = 0, ErrorMessage = null });
            }
            catch (Exception e)
            {
                return InternalServerError(new ResultApi { Result = null, ErrorCode = 687, ErrorMessage = e.Message });
            }
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
        [ProducesResponseType(typeof(ResultApi), 200)]
        [ProducesResponseType(typeof(ResultApi), 500)]
        [ProducesResponseType(typeof(ResultApi), 404)]
        [HttpDelete("{userId:long}/[action]")]
        public ActionResult<ResultApi> DeleteCard(long userId, [FromBody] string pan)
        {
            if (!_manager.UserExist(userId))
                return NotFound(new ResultApi { Result = null, ErrorCode = 800, ErrorMessage = "User not found." });

            if (!_manager.GetCards(userId).Any(c => c.Pan == pan))
                return NotFound(new ResultApi { Result = null, ErrorCode = 799, ErrorMessage = $"User has no current card with pan {pan}" });

            try
            {
                var cardDto = _manager.DeleteCard(userId, pan);

                return Ok(new ResultApi { Result = cardDto, ErrorCode = 0, ErrorMessage = null });
            }
            catch (Exception e)
            {
                return InternalServerError(new ResultApi { Result = null, ErrorCode = 687, ErrorMessage = e.Message });
            }
        }

        /// <summary>
        /// Get user cards
        /// </summary>
        /// <param name="userId">User id</param>
        /// <response code="200">Cards getted</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Cards dont added, server error</response>
        /// <returns>Result work server</returns>
        [ProducesResponseType(typeof(ResultApi), 200)]
        [ProducesResponseType(typeof(ResultApi), 500)]
        [ProducesResponseType(typeof(ResultApi), 404)]
        [HttpGet("{userId:long}/[action]")]
        public ActionResult<ResultApi> GetCards(long userId)
        {
            if (!_manager.UserExist(userId))
                return NotFound(new ResultApi { Result = null, ErrorCode = 800, ErrorMessage = "User not found." });

            try
            {
                var cards = _manager.GetCards(userId);

                return Ok(new ResultApi { Result = cards, ErrorCode = 0, ErrorMessage = null });
            }
            catch (Exception e)
            {
                return InternalServerError(new ResultApi { Result = null, ErrorCode = 687, ErrorMessage = e.Message });
            }

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
        [ProducesResponseType(typeof(ResultApi), 200)]
        [ProducesResponseType(typeof(ResultApi), 500)]
        [ProducesResponseType(typeof(ResultApi), 404)]
        [HttpPost("{userId:long}/[action]")]
        public ActionResult<ResultApi> GetCard([FromRoute] long userId, [FromBody] string pan)
        {
            if (!_manager.UserExist(userId))
                return NotFound(new ResultApi { Result = null, ErrorCode = 800, ErrorMessage = "User not found." });

            try
            {
                var card = _manager.GetCards(userId).FirstOrDefault(c => c.Pan == pan);

                if (card != null)
                    return Ok(new ResultApi { Result = card, ErrorCode = 0, ErrorMessage = null });
                else
                    return NotFound(new ResultApi { Result = null, ErrorCode = 799, ErrorMessage = $"User has no current card with pan {pan}" });

            }
            catch (Exception e)
            {
                return InternalServerError(new ResultApi { Result = null, ErrorCode = 687, ErrorMessage = e.Message });
            }
        }

        /// <summary>
        /// Change cardHolder on user cards
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cardHolderName">New nameholder on cards</param>
        /// <response code="200">Card changed</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Server error</response>
        /// <returns>Result work server</returns>
        [ProducesResponseType(typeof(ResultApi), 200)]
        [ProducesResponseType(typeof(ResultApi), 500)]
        [ProducesResponseType(typeof(ResultApi), 404)]
        [HttpPatch("{userId:long}/[action]")]
        public ActionResult<ResultApi> ChangeCardHolder([FromRoute] long userId, [FromBody] string cardHolderName)
        {
            if (!_manager.UserExist(userId))
                return NotFound(new ResultApi { Result = null, ErrorCode = 800, ErrorMessage = "User not found." });

            try
            {
                var cards = _manager.ChangeCardHolder(userId, cardHolderName);

                return Ok(new ResultApi { Result = cards, ErrorCode = 0, ErrorMessage = null });
            }
            catch (Exception e)
            {
                return InternalServerError(new ResultApi { Result = null, ErrorCode = 687, ErrorMessage = e.Message });
            }
        }

        /// <summary>
        /// Get Internal server error with code 500.
        /// </summary>
        private InternalServerErrorObjectResult InternalServerError(object value) => new InternalServerErrorObjectResult(value);

    }
}
