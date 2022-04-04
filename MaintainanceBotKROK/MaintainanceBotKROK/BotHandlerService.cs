using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types.Enums;

namespace MaintainanceBotKROK
{
    public sealed class BotHandlerService : BackgroundService
    {
        private readonly ITelegramBotClient _client;
        private readonly IUpdateHandler _updateHandler;

        public BotHandlerService(ITelegramBotClient client, IUpdateHandler updateHandler)
        {
            _client = client;
            _updateHandler = updateHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _client.ReceiveAsync(_updateHandler,
                new ReceiverOptions
                {
                    AllowedUpdates = new[] { UpdateType.Message }
                },
                stoppingToken);
        }
    }
}
