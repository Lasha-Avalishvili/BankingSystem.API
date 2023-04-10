using BankingSystem.DB.Entities;

namespace BankingSystem.Features.InternetBank.User.Transactions
{
    public class TransactionResponse
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public decimal Amount { get; set; }
        public string SenderAccount { get; set; }
        public string? RecipientAccount { get; set; }
        public Currency CurrencyFrom { get; set; }
        public Currency? CurrencyTo { get; set; }
    }
}
