using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Middleware.Data;
using Middleware.Data.Commands.Execute;
using Middleware.Data.Commands.Query;
using Middleware.Data.Dto;
using Middleware.Data.Model;
using Middleware.Dto;
using Middleware.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        private readonly IRepository _manager;
        private readonly IMapper _map;
        private readonly IMediator _mediator;

        public CardManagerController(IRepository cardManager, IMapper map, IMediator mediator)
        {
            _manager = cardManager;
            _map = map;
            _mediator = mediator;
        }

        [HttpGet("test")]
        public ActionResult<string> Test() => Ok(new ResultApi<string> { Result = "Success!" });

        /// <summary>
        /// Add new user card.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cardWriteDto">New user card</param>
        /// <response code="200">Card added</response>
        /// <response code="500">Card dont added, server error</response>
        /// <response code="404">Non used</response>
        /// <returns>Result work server</returns>
        [ProducesResponseType(typeof(ResultApi<CardReadDto>), 200)]
        [HttpPost("{userId:long}/[action]")]
        public async Task<ActionResult<ResultApi<CardReadDto>>> AddCard(long userId, CardWriteDto cardWriteDto)
        {
            cardWriteDto.userId = userId;
            var id = await _mediator.Send(new CreateCardCommand { Card = _map.Map<Card>(cardWriteDto) });
            var cardReadDto = _map.Map<CardReadDto>(await _manager.GetAsync<Card>(id));
            return new ResultApi<CardReadDto> { Result = cardReadDto, ErrorCode = 0, ErrorMessage = null };
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
        public async Task<ActionResult<ResultApi<CardReadDto>>> DeleteCard(long userId, [FromBody] string pan)
        {
            var card = await _mediator.Send(new GetCardForUserIdAndPanCommand { UserId = userId, Pan = pan });

            if (card == null)
                return NotFound(new ResultApi<string> { Result = userId.ToString(), ErrorCode = 800, ErrorMessage = "Card not found." });

            await _mediator.Send(new DeleteCardCommand { Card = card });

            var cardReadDto = _map.Map<CardReadDto>(card);
            return new ResultApi<CardReadDto> { Result = cardReadDto, ErrorCode = 0, ErrorMessage = null };
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
        public async Task<ActionResult<ResultApi<IEnumerable<CardReadDto>>>> GetCards(long userId)
        {
            var cards = await _mediator.Send(new GetCardsForUSerIdCommand { UserId = userId });

            if (!cards.Any())
                return NotFound(new ResultApi<string> { Result = userId.ToString(), ErrorCode = 800, ErrorMessage = "Cards not found." });

            var cardsReadDto = _map.Map<IEnumerable<CardReadDto>>(cards);
            return new ResultApi<IEnumerable<CardReadDto>> { Result = cardsReadDto, ErrorCode = 0, ErrorMessage = null };
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
        public async Task<ActionResult<ResultApi<CardReadDto>>> GetCard([FromRoute] long userId, [FromBody] string pan)
        {
            var card = await _mediator.Send(new GetCardForUserIdAndPanCommand { UserId = userId, Pan = pan });

            if (card == null)
                return NotFound(new ResultApi<string> { Result = userId.ToString(), ErrorCode = 800, ErrorMessage = "Card not found." });

            var cardReadDto = _map.Map<CardReadDto>(card);
            return new ResultApi<CardReadDto> { Result = cardReadDto, ErrorCode = 0, ErrorMessage = null };
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
        public async Task<ActionResult<ResultApi<IEnumerable<CardReadDto>>>> ChangeCardHolder([FromRoute] long userId, [FromBody] string cardHolderName)
        {
            if (long.TryParse(cardHolderName, out long c))
                return BadRequest(new ResultApi<string> { Result = userId.ToString(), ErrorCode = 801, ErrorMessage = "Wrong cardholder." });

            var cards = await _mediator.Send(new GetCardsForUSerIdCommand { UserId = userId });

            if (!cards.Any())
                return NotFound(new ResultApi<string> { Result = userId.ToString(), ErrorCode = 800, ErrorMessage = "Cards not found." });

            await _mediator.Send(new ChangeCardHolderNameCardsCommand { Cards = cards, CardHolderName = cardHolderName });

            var cardsReadDtos = _map.Map<IEnumerable<CardReadDto>>(cards);
            return new ResultApi<IEnumerable<CardReadDto>> { Result = cardsReadDtos, ErrorCode = 0, ErrorMessage = null };
        }
    }
}
