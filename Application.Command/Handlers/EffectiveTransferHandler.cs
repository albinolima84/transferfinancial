using Application.Command.Commands;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Command.Handlers
{
    public class EffectiveTransferHandler : IRequestHandler<EffectiveTransferCommand, Unit>
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<EffectiveTransferHandler> _logger;

        public EffectiveTransferHandler(ITransferRepository transferRepository, IAccountRepository accountRepository, ILogger<EffectiveTransferHandler> logger)
        {
            _transferRepository = transferRepository;
            _accountRepository = accountRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(EffectiveTransferCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Iniciando transferência de transação {request.TransactionId}");

            var transfer = new TransferFinancial(request.TransactionId, request.AccountOrigin, request.AccountDestination, request.Value, StatusEnum.Processing);

            _logger.LogInformation($"Atualizando o status da transação {request.TransactionId} para {transfer.Status}");

            await _transferRepository.UpdateStatus(transfer);

            _logger.LogInformation($"Efetivando a transferência da transação {transfer.TransactionId}");

            EffectiveTransferCommand(transfer);

            return Unit.Value;
        }

        private void EffectiveTransferCommand(TransferFinancial transfer)
        {
            string errorMessage = string.Empty;
            var statusTransfer = StatusEnum.Confirmed;

            var accountRequest = _accountRepository.VerifyAccount(transfer.AccountOrigin);
            if(accountRequest.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning($"Conta {transfer.AccountOrigin} não encontrada.");
                errorMessage = "Invalid account number";
                statusTransfer = StatusEnum.Error;
            }

            //if (accountRequest.StatusCode == HttpStatusCode.OK)
            //{
            //    _logger.LogWarning($"Conta {transfer.AccountOrigin} não encontrada.");
            //    errorMessage = "Invalid account number";
            //    statusTransfer = StatusEnum.Error;
            //}

            transfer.SetErrorMessage(errorMessage);
            transfer.SetNewStatus(statusTransfer);

            _transferRepository.UpdateStatus(transfer);
        }
    }
}
