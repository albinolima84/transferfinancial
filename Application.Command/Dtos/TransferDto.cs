namespace Application.Command.Dtos
{
    public class TransferDto
    {
        public string AccountOrigin { get; set; }
        public string AccountDestination { get; set; }
        public decimal Value { get; set; }
    }
}
