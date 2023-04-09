using BankingSystem.DB.Entities;

namespace BankingSystem.Features.ATM.AccountBlance
{
    public class GetBalanceResponse
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public decimal Balance { get; set; }
        public Currency Currency { get; set; }
    }
}
