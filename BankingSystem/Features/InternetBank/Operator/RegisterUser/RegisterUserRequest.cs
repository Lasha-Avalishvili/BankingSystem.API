using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Features.InternetBank.Operator.RegisterUser
{
    public class RegisterUserRequest
    {
        [Required (ErrorMessage = "FirstName Required")]
        [MinLength(1, ErrorMessage = "FirstName min length should be 1")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lastname Required")]
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
