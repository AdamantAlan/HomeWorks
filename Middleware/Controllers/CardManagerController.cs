using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Middleware.Data;
using Middleware.Dto;
using Middleware.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return Ok("Test success!");
        }

        [HttpPost("{userId:length(1,10)}/[action]")]
        public ActionResult<ResultApi> AddCard(long userId, Card card)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultApi { Result = null, ErrorCode = 0, ErrorMessage = "No valid data." });

            card.UserId = userId;

            _manager.SetCard(card);

            return Ok();
        }

        [HttpDelete("{userId:length(1,10)}/[action]")]
        public ActionResult<ResultApi> DeleteCard(long userId, [FromBody] string pan)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultApi { Result = null, ErrorCode = 0, ErrorMessage = "No valid data." });

            if (!_manager.UserExist(userId))
                return NotFound(new ResultApi { Result = null, ErrorCode = 800, ErrorMessage = "User not found." });

            if (!_manager.GetCards(userId).Any(c => c.Pan == pan))
                return NotFound(new ResultApi { Result = null, ErrorCode = 799, ErrorMessage = $"User has no current card with pan {pan}" });

            var deletedCard = _manager.DeleteCard(userId, pan);

            return Ok(new ResultApi { Result = deletedCard, ErrorCode = 0, ErrorMessage = null });
        }

        [HttpGet("{userId:length(1,10)}/[action]")]
        public ActionResult<ResultApi> GetCards(long userId)
        {
            if (!_manager.UserExist(userId))
                return NotFound(new ResultApi { Result = null, ErrorCode = 800, ErrorMessage = "User not found." });

            var cards = _manager.GetCards(userId);
            return Ok(new ResultApi { Result = cards, ErrorCode = 0, ErrorMessage = null });
        }

        [HttpPost("{userId:length(1,10)}/[action]")]
        public ActionResult<ResultApi> GetCard([FromRoute] long userId, [FromBody] string pan)
        {
            if (!_manager.UserExist(userId))
                return NotFound(new ResultApi { Result = null, ErrorCode = 800, ErrorMessage = "User not found." });

            var cards = _manager.GetCards(userId).FirstOrDefault(c => c.Pan == pan);

            if (cards != null)
                return Ok(new ResultApi { Result = cards, ErrorCode = 0, ErrorMessage = null });
            else
                return NotFound(new ResultApi { Result = null, ErrorCode = 799, ErrorMessage = $"User has no current card with pan {pan}" });
        }

        [HttpPatch("{userId:length(1,10)}/[action]")]
        public ActionResult<ResultApi> ChangeCardHolder([FromRoute] long userId, [FromBody] string cardHolderName)
        {
            if (!_manager.UserExist(userId))
                return NotFound(new ResultApi { Result = null, ErrorCode = 800, ErrorMessage = "User not found." });

            var cards = _manager.ChangeCardHolder(userId, cardHolderName);
            return Ok(new ResultApi { Result = cards, ErrorCode = 0, ErrorMessage = null });
        }
    }
}
