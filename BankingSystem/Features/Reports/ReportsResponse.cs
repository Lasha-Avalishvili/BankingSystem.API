using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Features.Reports
{
    public class ReportsResponse
    {
        public int UsersThisYear { get; set; }
        public int UsersInOneYear { get; set;}
        public int UsersInLast30Days { get; set; }
    }
}
