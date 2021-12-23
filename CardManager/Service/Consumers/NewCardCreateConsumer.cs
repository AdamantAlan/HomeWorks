using AutoMapper;
using Data.Data.MessageMQ;
using Data.Interfaces;
using Data.Model;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardManager.Service.Consumers
{
    /// <summary>
    /// Listens for messages about new cards and create their.
    /// </summary>
    [Obsolete("Old version")]
    public class NewCardCreateConsumerV1 : IConsumer<NewCardMessageV1>
    {
        private readonly IRepository _db;
        private readonly IMapper _map;

        public NewCardCreateConsumerV1(IRepository db, IMapper map)
        {
            _db = db;
            _map = map;
        }

        public async Task Consume(ConsumeContext<NewCardMessageV1> context)
        {
            var newCardModel = _map.Map<CardModel>(context.Message);
            var tempid = await _db.CreateAsync(newCardModel);
            await context.RespondAsync(new NewCardMessageResponse { Id = tempid });
        }
    }

    /// <summary>
    /// Listens for messages about new cards and create their.
    /// </summary>
    public class NewCardCreateConsumerV2 : IConsumer<NewCardMessageV2>
    {
        private readonly IRepository _db;
        private readonly IMapper _map;

        public NewCardCreateConsumerV2(IRepository db, IMapper map)
        {
            _db = db;
            _map = map;
        }

        public async Task Consume(ConsumeContext<NewCardMessageV2> context)
        {
            var newCardModel = _map.Map<CardModel>(context.Message);
            await _db.CreateAsync(newCardModel);
        }
    }
}
