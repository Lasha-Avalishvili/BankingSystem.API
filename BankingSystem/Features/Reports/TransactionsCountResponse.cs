using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Features.Reports
{
    public class TransactionsCountResponse
    {
        public TransactionsCountSummary UsersThisYear { get; set; }
        public TransactionsCountSummary UsersInOneYear { get; set; }
        public TransactionsCountSummary UsersInLast30Days { get; set; }
    }
}
