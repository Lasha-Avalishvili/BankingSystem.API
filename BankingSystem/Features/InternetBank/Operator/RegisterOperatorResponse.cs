using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.DB.Entities;

namespace BankingSystem.Features.InternetBank.Operator
{
    public class RegisterOperatorResponse
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public OperatorEntity Operator { get; set; }
    }
}
