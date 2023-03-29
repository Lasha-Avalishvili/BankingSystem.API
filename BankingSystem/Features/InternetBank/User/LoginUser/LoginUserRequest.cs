using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Features.InternetBank.User.LoginUser
{
    public class LoginUserRequest
    {
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Personal number must be 11 digits")]
        public string PersonalNumber { get; set; }

        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }
    }
}
