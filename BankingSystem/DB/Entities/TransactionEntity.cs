namespace BankingSystem.DB.Entities
{
    public enum TransactionType
    { 
        Inner,
        Outer,
        ATM
    }

    public class TransactionEntity
    {
        public long Id { get; set; }
        public long SenderAccountId { get; set; }   // change to int
        public long? RecipientAccountId { get; set; }  // change to int 
        public DateTime CreatedAt { get; set; }
        public decimal Amount { get; set; }
        public string SenderAccount { get; set; }
        public string? RecipientAccount { get; set;}
        public Currency CurrencyFrom { get; set; }
        public Currency? CurrencyTo { get; set; }
        public decimal? ConvertRate { get; set; }
        public decimal AmountInGEL { get; set; }
        public decimal FeeInGEL { get; set; }
        public decimal FeeInUSD { get; set; }
        public decimal FeeInEUR { get; set; } 
        public TransactionType TransactionType { get; set; }
    }
}
