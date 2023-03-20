using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Features.InternetBank.Operator.AddUser
{
    public class AddCardRequest
    {
        public int AccountId { get; set; }
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Card number must be 16 digits")]
        public string CardNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? FullName { get; set; }
        [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV must be a 3-digit number")]
        public string CVV { get; set; }
        [RegularExpression(@"^\d{4}$", ErrorMessage = "PIN must be a 4-digit number")]
        public string PIN { get; set; }
    }
}
