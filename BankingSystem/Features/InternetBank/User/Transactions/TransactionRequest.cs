using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Features.InternetBank.User.Transactions
{
    public class TransactionRequest
    {
        public string SenderAccountIBAN { get; set; }
        public string RecipientAccountIBAN { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Amount must be a valid number")]
        public decimal Amount { get; set; }

    }
}
