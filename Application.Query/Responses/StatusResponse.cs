namespace Application.Query.Responses
{
    public class StatusResponse
    {
        public string Status { get; }

        public string ErrorMessage { get; }

        public StatusResponse(string status, string errorMessage)
        {
            Status = status;
            ErrorMessage = errorMessage;
        }
    }
}
