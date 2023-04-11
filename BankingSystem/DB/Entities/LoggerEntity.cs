using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DB.Entities
{
    public class LoggerEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Exception { get; set; }
        public double? TransactionRequiredTime { get; set; }
    }
}
