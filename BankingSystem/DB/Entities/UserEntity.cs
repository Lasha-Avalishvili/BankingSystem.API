using Microsoft.AspNetCore.Identity;

namespace BankingSystem.DB.Entities
{
    public class UserEntity : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PersonalNumber { get; set; }
        public List<AccountEntity> Accounts { get; set; }
    }
}
