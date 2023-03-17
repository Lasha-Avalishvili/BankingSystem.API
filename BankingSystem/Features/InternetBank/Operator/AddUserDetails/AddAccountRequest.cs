using BankingSystem.DB.Entities;

namespace BankingSystem.Features.InternetBank.Operator.AddAccountForUser
{

    public class AddAccountRequest
    {
        public int UserId { get; set; }
        public string IBAN { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        
    }
}
