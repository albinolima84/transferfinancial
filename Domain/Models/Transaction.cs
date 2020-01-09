namespace Domain.Models
{
    public class Transaction
    {
        public string AccountNumber { get; set; }

        public decimal Value { get; set; }

        public string Type { get; set; }
    }
}
