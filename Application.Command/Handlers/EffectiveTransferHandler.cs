using Application.Command.Commands;
using Application.Command.Dtos;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
            try
            {
                _logger.LogInformation($"Iniciando transferência de transação {request.TransactionId}");

                var transfer = new TransferFinancial(request.TransactionId, request.AccountOrigin, request.AccountDestination, request.Value, StatusEnum.Processing);

                _logger.LogInformation($"Atualizando o status da transação {request.TransactionId} para {transfer.Status}");

                await _transferRepository.UpdateStatus(transfer);

                _logger.LogInformation($"Efetivando a transferência da transação {transfer.TransactionId}");

                EffectiveTransfer(transfer);

                return Unit.Value;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Erro ao efetivar a transferência {request.TransactionId}.");
                throw;
            }
        }

        private void EffectiveTransfer(TransferFinancial transfer)
        {
            string errorMessage = string.Empty;
            var statusTransfer = StatusEnum.Confirmed;

            var accountRequest = _accountRepository.VerifyAccount(transfer.AccountOrigin);
            
            if (accountRequest.StatusCode == HttpStatusCode.InternalServerError || accountRequest.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                _logger.LogInformation("Serviço de contas indisponível");

                transfer.SetNewStatus(StatusEnum.InQueue);
                _transferRepository.UpdateStatus(transfer);

                throw new Exception("Serviço de contas indisponível");
            }
            else if (accountRequest.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning($"Conta origem {transfer.AccountOrigin} não encontrada.");
                errorMessage = "Invalid origin account number";
                statusTransfer = StatusEnum.Error;
            }
            else if (accountRequest.StatusCode == HttpStatusCode.OK)
            {
                var account = JsonConvert.DeserializeObject<AccountDto>(accountRequest.Content);
                if (account.Balance < transfer.Value)
                {
                    _logger.LogWarning($"Conta origem {transfer.AccountOrigin} com saldo insuficiente: {account.Balance}.");
                    errorMessage = "Insufficient balance";
                    statusTransfer = StatusEnum.Error;
                }
                else
                {
                    var accountDestinationRequest = _accountRepository.VerifyAccount(transfer.AccountDestination);
                    if (accountDestinationRequest.StatusCode == HttpStatusCode.NotFound)
                    {
                        _logger.LogWarning($"Conta {transfer.AccountDestination} não encontrada.");
                        errorMessage = "Invalid destination account number";
                        statusTransfer = StatusEnum.Error;
                    }
                }
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                _logger.LogInformation($"Efetuando o débito na conta {transfer.AccountOrigin} no valor de {transfer.Value}");

                var bodyTransaction = new Transaction { AccountNumber = transfer.AccountOrigin, Value = transfer.Value, Type = "Debit"};
                var transaction = _accountRepository.EffectiveTransfer(bodyTransaction);

                _logger.LogInformation($"Efetuando o crédito na conta {transfer.AccountDestination} no valor de {transfer.Value}");

                bodyTransaction = new Transaction { AccountNumber = transfer.AccountDestination, Value = transfer.Value, Type = "Credit" };
                transaction = _accountRepository.EffectiveTransfer(bodyTransaction);
            }

            transfer.SetErrorMessage(errorMessage);
            transfer.SetNewStatus(statusTransfer);

            _transferRepository.UpdateStatus(transfer);
        }
    }
}
