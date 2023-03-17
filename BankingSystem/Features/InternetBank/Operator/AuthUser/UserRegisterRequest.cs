namespace BankingSystem.Features.InternetBank.Operator.RegisterUser
{
    public class UserRegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PersonalNumber { get; set; }
        public string Password { get; set; }
        
    }
}
