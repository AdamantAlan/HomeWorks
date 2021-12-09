using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Middleware.Data;
using Middleware.Data.Commands.Execute;
using Middleware.Data.Commands.Query;
using Middleware.Data.Dto;
using Middleware.Data.Model;
using Middleware.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Middleware.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _map;

        public TransactionController(IMediator mediator, IMapper map)
        {
            _mediator = mediator;
            _map = map;
        }

        [HttpGet("test")]
        public ActionResult<string> Test() => Ok(new ResultApi<string> { Result = "Success Transaction controller!" });

        /// <summary>
        /// Get operations of user cards.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <response code="200">Transactions getted</response>
        /// <response code="404">Transactions not found</response>
        /// <response code="500">Server error</response>
        /// <returns>Result work server</returns>
        [ProducesResponseType(typeof(ResultApi<IEnumerable<TransactionReadDto>>), 200)]
        [HttpPost("user/{userId:long}/[action]")]
        public async Task<ActionResult<ResultApi<IEnumerable<TransactionReadDto>>>> GetAllTransaction(long userId)
        {
            var transactions = await _mediator.Send(new GetAllTransactionsAsyncCommand { UserId = userId });
            if (!transactions.Any())
                return NotFound(new ResultApi<string> { Result = userId.ToString(), ErrorCode = 1800, ErrorMessage = "Transactions not found." });

            var transactionsReadDto = _map.Map<IEnumerable<TransactionReadDto>>(transactions);
            return new ResultApi<IEnumerable<TransactionReadDto>> { Result = transactionsReadDto, ErrorCode = 0, ErrorMessage = null };
        }

        /// <summary>
        /// Add operations of default user card.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="amount">money</param>
        /// <param name="transactionType">type of transacion</param>
        /// <response code="200">Transaction added</response>
        /// <response code="404">Default card not found</response>
        /// <response code="500">Server error</response>
        /// <returns>Result work server</returns>
        [ProducesResponseType(typeof(ResultApi<TransactionReadDto>), 200)]
        [HttpPost("user/{userId:long}/[action]")]
        public async Task<ActionResult<ResultApi<TransactionReadDto>>> AddTransactionByDefaultCard(long userId, decimal amount, byte transactionType)
        {
            var defaultCardId = await _mediator.Send(new GetDefaultCardIdAsyncCommand { UserId = userId });

            if (defaultCardId == 0)
                return NotFound(new ResultApi<string> { Result = null, ErrorCode = 1802, ErrorMessage = "Default card not found." });

            var idAddTransaction = await _mediator.Send(new CreateTransactionByCardAsyncCommand
            {
                UserId = userId,
                Amount = amount,
                TransactionType = (TransactionType)transactionType,
                CardId = defaultCardId
            });

            var transactionReadDto = _map.Map<TransactionReadDto>(await _mediator.Send(new GetTransactionByIdAsyncCommand { Id = idAddTransaction }));

            return new ResultApi<TransactionReadDto> { Result = transactionReadDto, ErrorCode = 0, ErrorMessage = null };
        }

        /// <summary>
        /// Add operations of user card.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="CardId">User id</param>
        /// <param name="amount">money</param>
        /// <param name="transactionType">type of transacion</param>
        /// <response code="200">Transaction added</response>
        /// <response code="404">Card not found</response>
        /// <response code="500">Server error</response>
        /// <returns>Result work server</returns>
        [ProducesResponseType(typeof(ResultApi<TransactionReadDto>), 200)]
        [HttpPost("user/{userId:long}/[action]")]
        public async Task<ActionResult<ResultApi<TransactionReadDto>>> AddTransactionByExistCard(long userId, long cardId, decimal amount, byte transactionType)
        {
            var isCardExist = await _mediator.Send(new CheckCardExistAsyncCommand { CardId = cardId });

            if (!isCardExist)
                return NotFound(new ResultApi<string> { Result = null, ErrorCode = 1803, ErrorMessage = "Card not found." });

            var idAddTransaction = await _mediator.Send(new CreateTransactionByCardAsyncCommand
            {
                UserId = userId,
                Amount = amount,
                TransactionType = (TransactionType)transactionType,
                CardId = cardId
            });

            var transactionReadDto = _map.Map<TransactionReadDto>(await _mediator.Send(new GetTransactionByIdAsyncCommand { Id = idAddTransaction }));

            return new ResultApi<TransactionReadDto> { Result = transactionReadDto, ErrorCode = 0, ErrorMessage = null };
        }

        /// <summary>
        /// Add operations of new user card.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="amount">money</param>
        /// <param name="transactionType">type of transacion</param>
        /// <param name="cardWriteDto">new user card</param>
        /// <response code="200">Transaction added</response>
        /// <response code="500">Server error</response>
        /// <returns>Result work server</returns>
        [ProducesResponseType(typeof(ResultApi<TransactionReadDto>), 200)]
        [HttpPost("user/{userId:long}/[action]")]
        public async Task<ActionResult<ResultApi<TransactionReadDto>>> AddTransactionByNewCard(long userId, decimal amount, byte transactionType, CardWriteDto cardWriteDto)
        {

            var cardId = await _mediator.Send(new CreateCardAsyncCommand { Card = _map.Map<Card>(cardWriteDto) });

            var idAddTransaction = await _mediator.Send(new CreateTransactionByCardAsyncCommand
            {
                UserId = userId,
                Amount = amount,
                TransactionType = (TransactionType)transactionType,
                CardId = cardId
            });

            var transactionReadDto = _map.Map<TransactionReadDto>(await _mediator.Send(new GetTransactionByIdAsyncCommand { Id = idAddTransaction }));

            return new ResultApi<TransactionReadDto> { Result = transactionReadDto, ErrorCode = 0, ErrorMessage = null };
        }
    }
}
