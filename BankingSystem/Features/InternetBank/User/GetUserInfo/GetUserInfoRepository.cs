using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Features.InternetBank.User.GetUserInfo
{
    public interface IGetUserInfoRepository
    {
        public Task<List<AccountEntity>> GetUserAccountsAsync(string userId);
        public Task<List<CardEntity>> GetUserCardsAsync(string authenticatedUserId, string Iban);
        public Task<AccountEntity> GetAccountWithIban(string iban);
        public Task<List<TransactionEntity>> GetTransactionsWithIban(string iban);
    }
    public class GetUserInfoRepository : IGetUserInfoRepository
    {
        private readonly AppDbContext _db;

        public GetUserInfoRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<List<AccountEntity>> GetUserAccountsAsync(string userId)
        {
            var accounts = _db.Accounts.Where(i => i.UserId.ToString() == userId).ToListAsync();
            return accounts;
        }

        public async Task<List<CardEntity>> GetUserCardsAsync(string authenticatedUserId, string Iban)
        {
            var cards = await (from c in _db.Cards
                               join a in _db.Accounts on c.AccountId equals a.Id
                               join u in _db.Users on a.UserId equals u.Id
                               where a.IBAN == Iban && u.Id == int.Parse(authenticatedUserId)
                               select c).ToListAsync();

            return cards;
        }


        public async Task<AccountEntity> GetAccountWithIban(string iban)
        {
            var account = await _db.Accounts.Where(a => a.IBAN == iban).FirstOrDefaultAsync();
            return account;
        }

        public async Task<List<TransactionEntity>> GetTransactionsWithIban(string iban)
        {
            var transactions = await _db.Transactions.Where(t => t.SenderAccount == iban || t.RecipientAccount == iban)
               .OrderByDescending(t => t.CreatedAt)
               .ToListAsync();
            return transactions;
        }

       
        
    }
}
