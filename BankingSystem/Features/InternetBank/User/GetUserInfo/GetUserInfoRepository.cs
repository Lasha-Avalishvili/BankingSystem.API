using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Features.InternetBank.User.GetUserInfo
{
    public interface IGetUserInfoRepository
    {
        Task<List<AccountEntity>> GetUserAccountsAsync(string userId);
        Task<List<CardEntity>> GetUserCardsAsync(string authenticatedUserId, int accountId);
        
        Task<List<TransactionEntity>> GetUserAccountTransactionsAsync(string iban);

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

        public Task<List<CardEntity>> GetUserCardsAsync(string authenticatedUserId, int accountId)
        {
            var cards = _db.Cards.Where(c => c.AccountId == accountId).ToListAsync();
            return cards;
        }

        public Task<List<TransactionEntity>> GetUserAccountTransactionsAsync(string iban)
        {
            var transactions = _db.Transactions
                .Where(t => t.SenderAccount == iban || t.RecipientAccount == iban)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return transactions;
        }
    }
}
