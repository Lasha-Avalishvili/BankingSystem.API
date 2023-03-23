using BankingSystem.DB;
using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.Operator.AddUser;
using IbanNet;

namespace BankingSystem.Features.InternetBank.Operator.AddAccountForUser
{
    public interface IAddUserDetailsRepository
    {
        public Task AddAccountAsync(AccountEntity entity);
        public Task AddCardAsync(CardEntity entity);
        public bool AccountExists(string iban); // use this in service for validation
    }
    public class AddUserDetailsRepository : IAddUserDetailsRepository
    {
        private readonly AppDbContext _db;
        public AddUserDetailsRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAccountAsync(AccountEntity entity)
        {
            await _db.Accounts.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
  
        public async Task AddCardAsync(CardEntity entity)
        {
            await _db.Cards.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public bool AccountExists(string iban)
        {
            return _db.Accounts.Any(a => a.IBAN == iban);
        }
       
    }
}
