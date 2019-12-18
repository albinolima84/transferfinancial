using Application.Command.Commands;
using CrossCutting.Options;
using Domain.Interfaces;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Api.BackgroundServices
{
    public class QueueConsumer : BackgroundService
    {
        private readonly IMediator _mediator;
        private readonly IQueueClient _queueClient;
        private readonly ILogger<QueueConsumer> _logger;
        private readonly MessagingConfigurationOptions _messagingConfiguration;
        private readonly ITransferRepository _transferRepository;

        public QueueConsumer(IMediator mediator, ILogger<QueueConsumer> logger, IOptions<MessagingConfigurationOptions> messagingConfiguration, ITransferRepository transferRepository)
        {
            _mediator = mediator;
            _logger = logger;
            _messagingConfiguration = messagingConfiguration.Value;
            _transferRepository = transferRepository;

            var connectionString = new ServiceBusConnectionStringBuilder(_messagingConfiguration.EndPoint, _messagingConfiguration.Queue, _messagingConfiguration.AccessKeyName, _messagingConfiguration.AccessKey);
            _queueClient = new QueueClient(connectionString);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            
            _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);

            return Task.CompletedTask;
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            _logger.LogError(arg.Exception, "Erro ao processar mensagem.");

            return Task.CompletedTask;
        }

        protected async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var messageBody = Encoding.UTF8.GetString(message.Body);
            _logger.LogInformation($"Iniciando processamento da mensagem {messageBody}");

            var transfer = JsonConvert.DeserializeObject<EffectiveTransferCommand>(messageBody);

            await _mediator.Send(transfer);

            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }
    }
}
