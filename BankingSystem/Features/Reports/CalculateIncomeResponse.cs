using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Features.Reports
{
    public class CalculateIncomeResponse
    {
        public IncomeCurrencies UsersThisYear { get; set; }
        public IncomeCurrencies UsersInOneYear { get; set; }
        public IncomeCurrencies UsersInLast30Days { get; set; }
    }
}
