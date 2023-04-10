using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Features.InternetBank.Operator.AddAccountForUser
{
    public interface IAddUserDetailsRepository
    {
        public Task AddAccountAsync(AccountEntity entity);
        public Task AddCardAsync(CardEntity entity);
        public Task<bool> AccountExists(string iban);
        public Task<bool> CardtExists(string cardNumber);
        public Task SaveChangesAsync();
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
            }
            public async Task AddCardAsync(CardEntity entity)
            {
                await _db.Cards.AddAsync(entity);
            }
            public async Task SaveChangesAsync()
            {
                await _db.SaveChangesAsync();
            }
            public async Task<bool> AccountExists(string iban)
            {
                return await _db.Accounts.AnyAsync(a => a.IBAN == iban);
            }

            public async Task<bool> CardtExists(string cardNumber)
            {
                return await _db.Cards.AnyAsync(a => a.CardNumber == cardNumber);
            }
        }
    }

