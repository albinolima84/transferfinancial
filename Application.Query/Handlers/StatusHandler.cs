using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Query.Requests;
using Application.Query.Responses;
using CrossCutting.Responses;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Query.Handlers
{
    public class StatusHandler : IRequestHandler<StatusQuery, Response<StatusResponse>>
    {
        private readonly ITransferRepository _transferRepository;
        private readonly ILogger<StatusHandler> _logger;

        public StatusHandler(ITransferRepository transferRepository, ILogger<StatusHandler> logger)
        {
            _transferRepository = transferRepository;
            _logger = logger;
        }

        public async Task<Response<StatusResponse>> Handle(StatusQuery request, CancellationToken cancellationToken)
        {
            try
            {
                StatusResponse response = null;

                _logger.LogInformation($"Iniciando pesquisa de transferência id {request.TransactionId}.");
                var transfer = await _transferRepository.GetTransfer(request.TransactionId);

                if (transfer != null)
                {
                    _logger.LogInformation($"Transferência {request.TransactionId} com status {transfer.Status}.");
                    response = new StatusResponse(transfer.Status.ToString(), transfer.ErrorMessage);
                }
                else
                {
                    _logger.LogWarning($"Nenhuma transferência com transactionId {request.TransactionId} foi encontrada.");
                }

                return Response<StatusResponse>.Ok(response);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Erro ao pesquisar pela transferência {request.TransactionId}");
                throw;
            }
        }
    }
}
