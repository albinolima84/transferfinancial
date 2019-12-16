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
                _logger.LogInformation($"Iniciando pesquisa de transferência id {request.TransactionId}");
                var status = await _transferRepository.GetTransfer(request.TransactionId);

                _logger.LogInformation($"Transferência {request.TransactionId} com status {status}");

                return Response<StatusResponse>.Ok(new StatusResponse(status, string.Empty));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Erro ao pesquisar pela transferência {request.TransactionId}");
                throw;
            }
        }
    }
}
