using BankingSystem.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Features.ATM.AccountBlance
{
    public class GetAccountBalanceResponse
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public decimal Balance { get; set; }
        public Currency Currency { get; set; }
    }
}
