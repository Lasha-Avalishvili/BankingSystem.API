using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Features.ATM.AccountBlance
{
    public class GetBalanceRequest
    {
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Card number must be 16 digits")]
        public string CardNumber { get; set; }
        [RegularExpression(@"^\d{4}$", ErrorMessage = "PIN must be a 4-digit number")]
        public string PIN { get; set; }
    }
}
