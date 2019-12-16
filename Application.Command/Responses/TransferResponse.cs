namespace Application.Command.Responses
{
    public class TransferResponse
    {
        public string TransactionId { get; }

        public TransferResponse(string transactionId)
        {
            TransactionId = transactionId;
        }
    }
}
