using Application.Command.Responses;
using CrossCutting.Responses;
using MediatR;

namespace Application.Command.Commands
{
    public class TransferCommand : IRequest<Response<TransferResponse>>
    {
        public string AccountOrigin { get; }
        public string AccountDestination { get; }
        public decimal Value { get; }

        private TransferCommand(string accountOrigin, string accountDestination, decimal value)
        {
            AccountOrigin = accountOrigin;
            AccountDestination = accountDestination;
            Value = value;
        }

        public static Response<TransferCommand> Create(string accountOrigin, string accountDestination, decimal value)
        {
            if (string.IsNullOrEmpty(accountOrigin))
            {
                return Response<TransferCommand>.Fail("conta_origem", "Conta origem inválida.");
            }

            if (string.IsNullOrEmpty(accountDestination))
            {
                return Response<TransferCommand>.Fail("conta_destino", "Conta destino inválida.");
            }

            if(accountOrigin == accountDestination)
            {
                return Response<TransferCommand>.Fail("conta_destino", "Conta destino deve ser diferente da conta origem.");
            }

            if (value == decimal.Zero)
            {
                return Response<TransferCommand>.Fail("conta_invalida", "Valor para transferência deve ser maior que zero.");
            }

            return Response<TransferCommand>.Ok(new TransferCommand(accountOrigin, accountDestination, value));
        }
    }
}
