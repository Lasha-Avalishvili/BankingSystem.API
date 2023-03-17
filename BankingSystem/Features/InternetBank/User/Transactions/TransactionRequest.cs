namespace BankingSystem.Features.InternetBank.User.Transactions
{
    public class TransactionRequest
    {
        public string SenderAccountIBAN { get; set; }
        public string RecipientAccountIBAN { get; set; }
        public decimal Amount { get; set; }

    }
}
