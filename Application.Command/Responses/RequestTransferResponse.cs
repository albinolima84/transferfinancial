namespace Application.Command.Responses
{
    public class RequestTransferResponse
    {
        public string TransactionId { get; }

        public RequestTransferResponse(string transactionId)
        {
            TransactionId = transactionId;
        }
    }
}
