using System;
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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Command.Handlers
{
    public class TransferHandler : IRequestHandler<TransferCommand, Response<TransferResponse>>
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IRabbitManager _manager;
        private readonly ILogger<TransferHandler> _logger;
        private readonly MessagingConfigurationOptions _messagingConfiguration;

        public TransferHandler(ITransferRepository transferRepository, ILogger<TransferHandler> logger, IRabbitManager manager, IOptions<MessagingConfigurationOptions> messagingConfiguration)
        {
            _transferRepository = transferRepository;
            _manager = manager;
            _logger = logger;
            _messagingConfiguration = messagingConfiguration.Value;
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

                _manager.Publish<TransferFinancial>(transfer, _messagingConfiguration.Exchange, _messagingConfiguration.ExchangeType, _messagingConfiguration.RoutingKey);

                return Response<TransferResponse>.Ok(new TransferResponse(transactionId));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Erro ao efetuar transferência da conta {request.AccountOrigin} para conta {request.AccountDestination} no valor de {request.Value}");
                throw;
            }
        }
    }
}
