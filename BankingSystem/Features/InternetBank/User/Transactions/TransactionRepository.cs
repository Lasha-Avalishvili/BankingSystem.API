using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Features.InternetBank.User.Transactions
{
    public interface ITransactionRepository
    {
        Task<AccountEntity> GetSenderAccountAsync(TransactionRequest transactionRequest);
        Task<AccountEntity> GetRecipientAccountAsync(TransactionRequest transactionRequest);
        Task SaveChangesAsync(TransactionEntity transaction);
    }
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _db;


        public TransactionRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<AccountEntity> GetSenderAccountAsync(TransactionRequest transactionRequest)
        {
            var senderAccount = await _db.Accounts.Where(sa => sa.IBAN == transactionRequest.SenderAccountIBAN).FirstOrDefaultAsync();

            if (senderAccount == null)
            {
                return null;
            }

            return senderAccount;
        }

        public async Task<AccountEntity> GetRecipientAccountAsync(TransactionRequest transactionRequest)
        {
            var recipientAccount = await _db.Accounts.Where(sr => sr.IBAN == transactionRequest.RecipientAccountIBAN).FirstOrDefaultAsync();
            if (recipientAccount == null)
            {
                return null;
            }

            return recipientAccount;
        }

        public async Task SaveChangesAsync(TransactionEntity transaction)
        {
            await _db.Transactions.AddAsync(transaction);

            await _db.SaveChangesAsync();
        }
    }
}
