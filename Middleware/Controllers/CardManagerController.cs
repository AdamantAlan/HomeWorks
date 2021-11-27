using Microsoft.AspNetCore.Mvc;
using Middleware.Data;
using Middleware.Interfaces;
using System;
using System.Linq;

namespace Middleware.Controllers
{
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

        [HttpPost("{userId:long}/[action]")]
        public ActionResult<ResultApi> AddCard(long userId, Card card)
        {
            card.UserId = userId;

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
