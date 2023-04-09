using BankingSystem.DB.Entities;

namespace BankingSystem.Features.ATM.Withdraw
{
    public class WithdrawResponse
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public decimal Amount { get; set; }
        public string AccountIBAN { get; set; }
        public Currency CurrencyFrom { get; set; }
        public Currency? CurrencyTo { get; set; }
    }
}
