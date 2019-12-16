namespace Application.Query.Responses
{
    public class StatusResponse
    {
        public string Status { get; }

        public StatusResponse(string status)
        {
            Status = status;
        }
    }
}
