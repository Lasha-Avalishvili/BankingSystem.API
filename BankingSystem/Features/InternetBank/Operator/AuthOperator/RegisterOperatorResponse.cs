
namespace BankingSystem.Features.InternetBank.Operator.AuthOperator
{
    public class RegisterOperatorResponse
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
