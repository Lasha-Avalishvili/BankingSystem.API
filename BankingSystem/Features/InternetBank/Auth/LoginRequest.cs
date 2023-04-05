using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Features.InternetBank.Auth
{
    public class LoginRequest
    {
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }
    }
}
