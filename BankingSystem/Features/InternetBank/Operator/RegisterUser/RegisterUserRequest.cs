using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Features.InternetBank.Operator.RegisterUser
{
    public class RegisterUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress (ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Personal number must be 11 digits")]
        public string PersonalNumber { get; set; }
        [MinLength(6,  ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }
    }
}
