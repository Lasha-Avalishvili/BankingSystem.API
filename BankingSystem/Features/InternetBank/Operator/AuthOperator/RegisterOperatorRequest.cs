using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Features.InternetBank.Operator.AuthOperator
{
    public class RegisterOperatorRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Personal number must have 11 digits")]
        public string PersonalNumber { get; set; }
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }
    }
}
