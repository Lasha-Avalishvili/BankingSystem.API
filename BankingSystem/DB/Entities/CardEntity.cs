namespace BankingSystem.DB.Entities
{
    public class CardEntity
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string CardNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? FullName { get; set; }
        public int CVV { get; set; }
        public string PIN { get; set; }
        public AccountEntity Account { get; set; }
    }
}
