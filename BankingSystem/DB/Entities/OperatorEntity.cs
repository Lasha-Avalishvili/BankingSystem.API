using Microsoft.AspNetCore.Identity;

namespace BankingSystem.DB.Entities
{
    public class OperatorEntity : IdentityUser<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PersonalNumber { get; set; }
    }
}
