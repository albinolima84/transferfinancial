using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Command.Commands;
using Application.Command.Responses;
using CrossCutting.Responses;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Command.Handlers
{
    public class TransferHandler : IRequestHandler<TransferCommand, Response<TransferResponse>>
    {
        private readonly ITransferRepository _transferRepository;
        private readonly ILogger<TransferHandler> _logger;

        public TransferHandler(ITransferRepository transferRepository, ILogger<TransferHandler> logger)
        {
            _transferRepository = transferRepository;
            _logger = logger;
        }

        public async Task<Response<TransferResponse>> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Inicia transferência da conta {request.AccountOrigin} para conta {request.AccountDestination} no valor de {request.Value}");

                var newTransactionId = Guid.NewGuid().ToString();

                var transfer = new TransferFinancial(newTransactionId, request.AccountOrigin, request.AccountDestination, request.Value, StatusEnum.InQueue);

                var transactionId = await _transferRepository.Transfer(transfer);

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
