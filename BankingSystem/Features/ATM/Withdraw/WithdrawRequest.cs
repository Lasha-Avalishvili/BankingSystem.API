using System.ComponentModel.DataAnnotations;
using BankingSystem.DB.Entities;

namespace BankingSystem.Features.ATM.Withdraw
{
    public class WithdrawRequest
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Amount must be a valid number")]
        [Range(typeof(decimal), "0.01", "1000000000000", ErrorMessage = "Amount must be greater than zero.")]

        public decimal Amount { get; set; }

        [Range(0, 3, ErrorMessage = "Currency must be a number between 0 and 3")]
        public Currency Currency { get; set; }

        [RegularExpression(@"^\d{16}$", ErrorMessage = "Card number must be 16 digits")]
        public string CardNumber { get; set; }
        [RegularExpression(@"^\d{4}$", ErrorMessage = "PIN must be 4 digits")]
        public string PIN { get; set; }
    }
}
