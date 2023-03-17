using Microsoft.AspNetCore.Identity;

namespace BankingSystem.DB.Entities
{
    public class UserEntity : IdentityUser<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PersonalNumber { get; set; }
        public string Password { get; set; }
        public List<AccountEntity> Accounts { get; set; }

    }
}
