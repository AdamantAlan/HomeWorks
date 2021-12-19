using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Data;
using Data.Commands.Execute;
using Data.Commands.Query;
using Data.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Data.Data.MessageMQ;
using System;
using TransactionManager.Services;

namespace TransactionManager.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ResultApi<string>), 401)]
    [ProducesResponseType(typeof(ResultApi<string>), 500)]
    [ProducesResponseType(typeof(ResultApi<string>), 404)]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _map;
        private readonly IRequestClient<NewCardMessageV1> _client;
        private readonly IBus _bus;
        private readonly IHttpClientForServiceCardManager _cardService;

        public TransactionController(IMediator mediator, IMapper map, IRequestClient<NewCardMessageV1> client, IBus bus, IHttpClientForServiceCardManager cardService)
        {
            _mediator = mediator;
            _map = map;
            _client = client;
            _bus = bus;
            _cardService = cardService;
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
        [HttpGet("user/{userId:long}/[action]")]
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
            var defaultCardId = await _cardService.GetIdDefaultUserCardAsync(userId);

            if (defaultCardId < 0)
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
        /// <param name="cardId">Card id</param>
        /// <param name="amount">money</param>
        /// <param name="transactionType">type of transacion</param>
        /// <response code="200">Transaction added</response>
        /// <response code="404">Card not found</response>
        /// <response code="500">Server error</response>
        /// <returns>Result work server</returns>
        [ProducesResponseType(typeof(ResultApi<TransactionReadDto>), 200)]
        [HttpPost("user/{userId:long}/[action]")]
        public async Task<ActionResult<ResultApi<TransactionReadDto>>> AddTransactionByExistCard(long userId, [FromBody] long cardId, decimal amount, byte transactionType)
        {
            var isCardExist = await _cardService.CheckExistUserCardAsync(cardId);
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
        /// <param name="transactionType">type of transaction</param>
        /// <param name="cardWriteDto">new user card</param>
        /// <response code="200">Transaction added</response>
        /// <response code="500">Server error</response>
        /// <returns>Result work server</returns>
        [Obsolete("Old version")]
        [ProducesResponseType(typeof(ResultApi<TransactionReadDto>), 200)]
        [HttpPost("user/{userId:long}/[action]")]
        public async Task<ActionResult<ResultApi<TransactionReadDto>>> AddTransactionByNewCardV1(long userId, decimal amount, byte transactionType, CardWriteDto cardWriteDto)
        {
            cardWriteDto.userId = userId;

            var cardid = await _client.GetResponse<NewCardMessageResponse>(_map.Map<NewCardMessageV1>(cardWriteDto));

            var idAddTransaction = await _mediator.Send(new CreateTransactionByCardAsyncCommand
            {
                UserId = userId,
                Amount = amount,
                TransactionType = (TransactionType)transactionType,
                CardId = cardid.Message.Id
            });

            var transactionReadDto = _map.Map<TransactionReadDto>(await _mediator.Send(new GetTransactionByIdAsyncCommand { Id = idAddTransaction }));

            return new ResultApi<TransactionReadDto> { Result = transactionReadDto, ErrorCode = 0, ErrorMessage = null };
        }

        /// <summary>
        /// Add operations of new user card.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cardId">Card id</param>
        /// <param name="amount">money</param>
        /// <param name="transactionType">type of transaction</param>
        /// <param name="cardWriteDto">new user card</param>
        /// <response code="200">Transaction added</response>
        /// <response code="500">Server error</response>
        /// <returns>Result work server</returns>
        [ProducesResponseType(typeof(ResultApi<TransactionReadDto>), 200)]
        [HttpPost("user/{userId:long}/[action]")]
        public async Task<ActionResult<ResultApi<TransactionReadDto>>> AddTransactionByNewCardV2(long userId, decimal amount, byte transactionType, long cardId, CardWriteDto cardWriteDto)
        {
            cardWriteDto.userId = userId;
            var message = _map.Map<NewCardMessageV2>(cardWriteDto);
            message.Id = cardId;
            await _bus.Publish(message);

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
