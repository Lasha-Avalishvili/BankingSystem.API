using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.DB.Entities;

namespace BankingSystem.Features.InternetBank.User.GetUserInfo
{
    public class GetTransactionsResponse
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime TransactionDate { get; set; }   
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public string SenderAccount { get; set; }
        public string RecipientAccount { get; set; }

    }
}
