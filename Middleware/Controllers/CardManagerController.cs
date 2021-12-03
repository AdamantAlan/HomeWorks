using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Middleware.Data;
using Middleware.Data.DbContexts;
using Middleware.Dto;
using Middleware.Extensions;
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

        public CardManagerController(IRepository cardManager, IMapper map)
        {
            _manager = cardManager;
            _map = map;
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
        public async Task<ActionResult<ResultApi<object>>> AddCard(long userId, CardWriteDto cardWriteDto)
        {
            cardWriteDto.userId = userId;
            var id = await _manager.CreateAsync(_map.Map<Card>(cardWriteDto));

            var cardReadDto = _map.Map<CardReadDto>(await _manager.GetAsync<Card>(id));
            return Ok(new ResultApi<CardReadDto> { Result = cardReadDto, ErrorCode = 0, ErrorMessage = null });
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
        public async Task<ActionResult<ResultApi<object>>> DeleteCard(long userId, [FromBody] string pan)
        {
            var card = _manager.GetAll<Card>().FirstOrDefault(c => c.UserId == userId && c.Pan == pan);

            if (card == null)
                return NotFound(new ResultApi<string> { Result = userId.ToString(), ErrorCode = 800, ErrorMessage = "Card not found." });

            await _manager.DeleteAsync(card);

            var cardReadDto = _map.Map<CardReadDto>(card);
            return Ok(new ResultApi<CardReadDto> { Result = cardReadDto, ErrorCode = 0, ErrorMessage = null });
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
        public async Task<ActionResult<ResultApi<IEnumerable<object>>>> GetCards(long userId)
        {
            var cards = await _manager.GetAll<Card>().Where(c => c.UserId == userId).ToListAsync();

            if (!cards.Any())
                return NotFound(new ResultApi<string> { Result = userId.ToString(), ErrorCode = 800, ErrorMessage = "Cards not found." });

            var cardsReadDto = _map.Map<IEnumerable<CardReadDto>>(cards);
            return Ok(new ResultApi<IEnumerable<CardReadDto>> { Result = cardsReadDto, ErrorCode = 0, ErrorMessage = null });
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
        public ActionResult<ResultApi<object>> GetCard([FromRoute] long userId, [FromBody] string pan)
        {
            var card = _manager.GetAll<Card>().FirstOrDefault(c => c.UserId == userId && c.Pan == pan);
            if (card == null)
                return NotFound(new ResultApi<string> { Result = userId.ToString(), ErrorCode = 800, ErrorMessage = "Card not found." });

            var cardReadDto = _map.Map<CardReadDto>(card);
            return Ok(new ResultApi<CardReadDto> { Result = cardReadDto, ErrorCode = 0, ErrorMessage = null });
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
        public async Task<ActionResult<ResultApi<IEnumerable<object>>>> ChangeCardHolder([FromRoute] long userId, [FromBody] string cardHolderName)
        {

            if (long.TryParse(cardHolderName, out long c))
                return BadRequest(new ResultApi<string> { Result = userId.ToString(), ErrorCode = 801, ErrorMessage = "Wrong cardholder." });

            var cards = await _manager.GetAll<Card>().Where(c => c.UserId == userId).ToListAsync();
            if (!cards.Any())
                return NotFound(new ResultApi<string> { Result = userId.ToString(), ErrorCode = 800, ErrorMessage = "Cards not found." });

            foreach (Card card in cards)
            {
                card.Name = cardHolderName;
            }

            if (!(await _manager.SaveChangeAsync()))
                return new InternalServerErrorObjectResult(new ResultApi<string> { Result = userId.ToString(), ErrorCode = 812, ErrorMessage = "Today change cardholder is lock." });

            var cardsReadDtos = _map.Map<IEnumerable<CardReadDto>>(cards);
            return Ok(new ResultApi<IEnumerable<CardReadDto>> { Result = cardsReadDtos, ErrorCode = 0, ErrorMessage = null });
        }
    }
}
