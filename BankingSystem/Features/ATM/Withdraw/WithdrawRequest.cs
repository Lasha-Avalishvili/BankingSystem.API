using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Features.ATM.Withdraw
{
    public class WithdrawRequest
    {
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
        public string PIN { get; set; }
    }
}
