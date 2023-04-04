using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Features.InternetBank.User.LoginUser
{
    public class LoginUserRequest
    {
         [EmailAddress(ErrorMessage ="Invalid email")]
        public string Email { get; set; }

        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }
    }
}
