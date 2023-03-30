using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Features.InternetBank.User.Transactions
{
    public interface ITransactionRepository
    {
        Task<AccountEntity> GetAccountAsync(string Iban);
        Task SaveChangesAsync(TransactionEntity transaction);
    }
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _db;


        public TransactionRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<AccountEntity> GetAccountAsync(string Iban)
        {
            var account = await _db.Accounts.Where(sr => sr.IBAN == Iban).FirstOrDefaultAsync();
            if (account == null)
            {
                return null;
            }

            return account;
        }

        public async Task SaveChangesAsync(TransactionEntity transaction)
        {
            await _db.Transactions.AddAsync(transaction);

            await _db.SaveChangesAsync();
        }
    }
}
