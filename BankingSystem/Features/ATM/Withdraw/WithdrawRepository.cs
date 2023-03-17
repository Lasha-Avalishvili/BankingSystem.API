using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Features.ATM.Withdraw
{
    public interface IWithdrawRepository
    {
        Task<AccountEntity> GetSenderAccountAsync(WithdrawRequest withdrawRequest);
        Task<List<TransactionEntity>> GetCurrentDayTransactionsForUser(long userId);
    }

    public class WithdrawRepository : IWithdrawRepository
    {
        private readonly AppDbContext _db;

        public WithdrawRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<AccountEntity> GetSenderAccountAsync(WithdrawRequest withdrawRequest)
        {
            var card = await _db.Cards.FirstOrDefaultAsync(c => c.CardNumber == withdrawRequest.CardNumber);
            var senderAccount = await _db.Accounts.FirstOrDefaultAsync(a => a.Id == card.AccountId);
            //validate account and card

            return senderAccount;
        }

        public async Task<List<TransactionEntity>> GetCurrentDayTransactionsForUser(long userId)
        {
            DateTime currentDate = DateTime.UtcNow.Date;
            var transactions = await _db.Transactions
                .Where(t => t.SenderAccountId == userId)
                .Where(t => t.TransactionType == TransactionType.ATM)
                .Where(t =>t.CreatedAt == currentDate).ToListAsync();

            return transactions;
        }
    }
}
