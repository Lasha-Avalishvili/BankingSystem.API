using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Features.Reports
{
    public class GetIncomeFromFeeResponse
    {
        public decimal FeeInGel { get; set; }
        public decimal FeeInUsd { get; set; }
        public decimal FeeInEur { get; set; }
    }
}
