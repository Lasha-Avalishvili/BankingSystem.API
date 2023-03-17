namespace BankingSystem.Features.InternetBank.Operator.AddUser
{
    public class AddCardRequest
    {
        public int AccountId { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? FullName { get; set; }
        public int CVV { get; set; }
        public string PIN { get; set; }
    }
}
