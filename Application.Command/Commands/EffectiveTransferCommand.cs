using Domain.Enums;
using MediatR;

namespace Application.Command.Commands
{
    public class EffectiveTransferCommand : IRequest<Unit>
    {
        public string TransactionId { get; set; }
        public string AccountOrigin { get; set; }
        public string AccountDestination { get; set; }
        public decimal Value { get; set; }
        public StatusEnum Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}
