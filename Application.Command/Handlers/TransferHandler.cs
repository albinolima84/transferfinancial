using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Command.Commands;
using Application.Command.Responses;
using CrossCutting.Options;
using CrossCutting.Responses;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Application.Command.Handlers
{
    public class TransferHandler : IRequestHandler<TransferCommand, Response<TransferResponse>>
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IQueueClient _queueClient;
        private readonly ILogger<TransferHandler> _logger;
        private readonly MessagingConfigurationOptions _messagingConfiguration;

        public TransferHandler(ITransferRepository transferRepository, ILogger<TransferHandler> logger, IOptions<MessagingConfigurationOptions> messagingConfiguration)
        {
            _transferRepository = transferRepository;
            _logger = logger;
            _messagingConfiguration = messagingConfiguration.Value;
            var connectionString = new ServiceBusConnectionStringBuilder("https://acesso.servicebus.windows.net/", "transferfinancial", "RootManageSharedAccessKey", "RLAspLQKYCV2sUaW/8Ylp+vt+rvpNNcbfnmdcXlvoDs=");
            //new QueueClient(ServiceBusConnectionStringBuilder)
            //_queueClient = new QueueClient(_messagingConfiguration.ConnectionString, _messagingConfiguration.Queue);
            _queueClient = new QueueClient(connectionString);
        }

        public async Task<Response<TransferResponse>> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Inicia transferência da conta {request.AccountOrigin} para conta {request.AccountDestination} no valor de {request.Value}");
                var statusTransfer = StatusEnum.InQueue;

                //var accountRequest = _accountRepository.VerifyAccount(request.AccountOrigin);
                //if(accountRequest.StatusCode == HttpStatusCode.NotFound)
                //{
                //    _logger.LogWarning($"Conta {request.AccountOrigin} não encontrada.");
                //    errorMessage = "Invalid account number";
                //    statusTransfer = StatusEnum.Error;
                //}

                var newTransactionId = Guid.NewGuid().ToString();

                var transfer = new TransferFinancial(newTransactionId, request.AccountOrigin, request.AccountDestination, request.Value, statusTransfer);

                var transactionId = await _transferRepository.Transfer(transfer);

                await SendMessagesAsync(transfer);

                return Response<TransferResponse>.Ok(new TransferResponse(transactionId));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Erro ao efetuar transferência da conta {request.AccountOrigin} para conta {request.AccountDestination} no valor de {request.Value}");
                throw;
            }
        }

        private async Task SendMessagesAsync(TransferFinancial transfer)
        {
            try
            {
                var messageBody = JsonConvert.SerializeObject(transfer);
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                _logger.LogInformation($"Enviando mensagem: {messageBody}");

                await _queueClient.SendAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}
