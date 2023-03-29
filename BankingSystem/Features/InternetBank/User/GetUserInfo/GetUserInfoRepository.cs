using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Features.InternetBank.User.GetUserInfo
{
    public interface IGetUserInfoRepository
    {
        Task<List<AccountEntity>> GetUserAccountsAsync(string userId);
        Task<List<CardEntity>> GetUserCardsAsync(string authenticatedUserId, int accountId);
        
        Task<List<TransactionEntity>> GetUserAccountTransactionsAsync(string iban, string authenticationUseId);

        //bool UserExists(string cardNumber);
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

        public async Task<List<CardEntity>> GetUserCardsAsync(string authenticatedUserId, int accountId)
        {
            var cards = await (from c in _db.Cards
                               join a in _db.Accounts on c.AccountId equals a.Id
                               join u in _db.Users on a.UserId equals u.Id
                               where a.Id == accountId && u.Id == int.Parse(authenticatedUserId)
                               select c).ToListAsync();

            return cards;
        }

        public async Task<List<TransactionEntity>> GetUserAccountTransactionsAsync(string iban, string authenticationUserId)
        {
            var transactions = await _db.Transactions
                .Where(t => (t.SenderAccount == iban || t.RecipientAccount == iban) && t.SenderAccountId == long.Parse(authenticationUserId))
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return transactions;
        }
    }
}
