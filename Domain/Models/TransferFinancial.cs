using Domain.Enums;

namespace Domain.Models
{
    public class TransferFinancial
    {
        public string TransactionId { get; }
        public string AccountOrigin { get; }
        public string AccountDestination { get; }
        public decimal Value { get; }
        public StatusEnum Status { get; }

        public TransferFinancial(string transactionId, string accountOrigin, string accountDestination, decimal value, StatusEnum status)
        {
            TransactionId = transactionId;
            AccountOrigin = accountOrigin;
            AccountDestination = accountDestination;
            Value = value;
            Status = status;
        }
    }
}
