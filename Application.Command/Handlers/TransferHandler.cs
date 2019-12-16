using System;
using System.Net;
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
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<TransferHandler> _logger;

        public TransferHandler(ITransferRepository transferRepository, ILogger<TransferHandler> logger, IAccountRepository accountRepository)
        {
            _transferRepository = transferRepository;
            _accountRepository = accountRepository;
            _logger = logger;
        }

        public async Task<Response<TransferResponse>> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Inicia transferência da conta {request.AccountOrigin} para conta {request.AccountDestination} no valor de {request.Value}");
                string errorMessage = string.Empty;
                var statusTransfer = StatusEnum.InQueue;

                var accountRequest = _accountRepository.VerifyAccount(request.AccountOrigin);
                if(accountRequest.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"Conta {request.AccountOrigin} não encontrada.");
                    errorMessage = "Invalid account number";
                    statusTransfer = StatusEnum.Error;
                }

                var newTransactionId = Guid.NewGuid().ToString();

                var transfer = new TransferFinancial(newTransactionId, request.AccountOrigin, request.AccountDestination, request.Value, statusTransfer, errorMessage);

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
