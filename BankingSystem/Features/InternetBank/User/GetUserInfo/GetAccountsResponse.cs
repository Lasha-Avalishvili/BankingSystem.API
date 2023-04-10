using BankingSystem.DB.Entities;

namespace BankingSystem.Features.InternetBank.User.GetUserInfo
{
    public class GetAccountsResponse 
    {
        public bool IsSuccessful { get; set; } 
        public string? ErrorMessage { get; set; }
        public List<AccountObject> Accounts { get; set; }
    }

    public class AccountObject
    {
        public int AccountId { get; set; }
        public string IBAN { get; set; }
        public decimal Balance { get; set; }
        public Currency Currency { get; set; }
    }
}
