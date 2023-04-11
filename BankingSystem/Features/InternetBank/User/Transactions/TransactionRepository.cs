using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Features.InternetBank.User.Transactions
{
    public interface ITransactionRepository
    {
        public Task<AccountEntity> GetAccountAsync(string Iban);
        public Task<List<ExchangeRateEntity>> GetCurrenciesAsync();
        public Task AddChangesAsync(TransactionEntity transaction);
        public Task SaveChangesAsync();
        public Task AddLogAsync(LoggerEntity log);
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
            return await _db.Accounts.Where(sr => sr.IBAN == Iban).FirstOrDefaultAsync();
        }

        public async Task<List<ExchangeRateEntity>> GetCurrenciesAsync()
        {
            var currencyRates = await _db.ExchangeRates.ToListAsync();
            return currencyRates;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task AddChangesAsync(TransactionEntity transaction)
        {
            await _db.Transactions.AddAsync(transaction);
        }

        public async Task AddLogAsync(LoggerEntity log)
        {
            await _db.Logs.AddAsync(log);
        }
    }
}
