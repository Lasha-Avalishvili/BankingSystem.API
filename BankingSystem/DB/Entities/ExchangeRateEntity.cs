using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DB.Entities
{
    public class ExchangeRateEntity
    {
        public int id { get; set; }
        public string QuoteCurrency { get; set; }
        public decimal Rate { get; set; }
    }
}
