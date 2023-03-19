using BankingSystem.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Features.InternetBank.User.Transactions
{
    public class TransactionResponse
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public decimal Amount { get; set; }
        public string SenderAccount { get; set; }
        public string? RecipientAccount { get; set; }
        public Currency CurrencyFrom { get; set; }
        public Currency? CurrencyTo { get; set; }
    }
}
