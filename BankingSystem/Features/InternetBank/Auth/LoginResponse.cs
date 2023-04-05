using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Features.InternetBank.Auth
{
    public class LoginResponse
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public string? JWT { get; set; }
    }
}
