using Application.Command.Responses;
using CrossCutting.Responses;
using MediatR;

namespace Application.Command.Commands
{
    public class RequestTransferCommand : IRequest<Response<RequestTransferResponse>>
    {
        public string AccountOrigin { get; }
        public string AccountDestination { get; }
        public decimal Value { get; }

        private RequestTransferCommand(string accountOrigin, string accountDestination, decimal value)
        {
            AccountOrigin = accountOrigin;
            AccountDestination = accountDestination;
            Value = value;
        }

        public static Response<RequestTransferCommand> Create(string accountOrigin, string accountDestination, decimal value)
        {
            if (string.IsNullOrEmpty(accountOrigin))
            {
                return Response<RequestTransferCommand>.Fail("conta_origem", "Conta origem inválida.");
            }

            if (string.IsNullOrEmpty(accountDestination))
            {
                return Response<RequestTransferCommand>.Fail("conta_destino", "Conta destino inválida.");
            }

            if(accountOrigin == accountDestination)
            {
                return Response<RequestTransferCommand>.Fail("conta_destino", "Conta destino deve ser diferente da conta origem.");
            }

            if (value == decimal.Zero)
            {
                return Response<RequestTransferCommand>.Fail("conta_invalida", "Valor para transferência deve ser maior que zero.");
            }

            return Response<RequestTransferCommand>.Ok(new RequestTransferCommand(accountOrigin, accountDestination, value));
        }
    }
}
