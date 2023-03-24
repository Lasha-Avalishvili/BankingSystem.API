using BankingSystem.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
