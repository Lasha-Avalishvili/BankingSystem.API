using BankingSystem.DB.Entities;

namespace BankingSystem.Features.InternetBank.User.GetUserInfo
{
    public class GetTransactionsResponse
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public List<TransactionObject> TransactionReponse {get; set;}
    }
    public class TransactionObject
    {
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public string SenderAccount { get; set; }
        public string RecipientAccount { get; set; }
    }
}
