using Application.Query.Responses;
using CrossCutting.Responses;
using MediatR;

namespace Application.Query.Requests
{
    public class StatusQuery : IRequest<Response<StatusResponse>>
    {
        public string TransactionId { get; }

        public StatusQuery(string transactionId)
        {
            TransactionId = transactionId;
        }
    }
}
