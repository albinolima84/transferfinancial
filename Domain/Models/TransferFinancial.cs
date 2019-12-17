using Domain.Enums;

namespace Domain.Models
{
    public class TransferFinancial
    {
        public string TransactionId { get; }
        public string AccountOrigin { get; }
        public string AccountDestination { get; }
        public decimal Value { get; }
        public StatusEnum Status { get; private set; }
        public string ErrorMessage { get; private set; }

        public TransferFinancial(string transactionId, string accountOrigin, string accountDestination, decimal value, StatusEnum status)
        {
            TransactionId = transactionId;
            AccountOrigin = accountOrigin;
            AccountDestination = accountDestination;
            Value = value;
            Status = status;
        }

        public TransferFinancial(string transactionId, string accountOrigin, string accountDestination, decimal value, StatusEnum status, string errorMessage) : this (transactionId, accountOrigin, accountDestination, value, status)
        {
            ErrorMessage = errorMessage;
        }

        public void SetNewStatus(StatusEnum newStatus)
        {
            Status = newStatus;
        }

        public void SetErrorMessage(string messageError)
        {
            ErrorMessage = messageError;
        }
    }
}
