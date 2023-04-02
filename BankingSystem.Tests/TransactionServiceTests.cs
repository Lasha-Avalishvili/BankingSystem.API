using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.DB.Entities;
using BankingSystem.DB;
using BankingSystem.Features.Reports;
using Microsoft.EntityFrameworkCore;
using BankingSystem.Features.InternetBank.User.Transactions;
using BankingSystem.Features.ATM.Withdraw;

namespace BankingSystem.Tests
{
    public class TransactionServiceTests
    {
        [Test]
        public async Task TransferFundsTest()
        {
            TransactionRequest transactionRequest = new TransactionRequest()
            {
                SenderAccountIBAN = "1", RecipientAccountIBAN = "2", Amount = 100
            };

            List<AccountEntity> accounts = new List<AccountEntity>()
            {
                new AccountEntity() { Balance = 200, Currency= 0, IBAN="1", Id=1, UserId= 1},
                new AccountEntity() { Balance = 0, Currency= 0, IBAN="2", Id=2, UserId= 2}
            };


            using var db = new AppDbContext(GetDbContextOptions());
            db.Database.EnsureCreated();
            db.Accounts.AddRange(accounts);
            await db.SaveChangesAsync();
            var transactionRepository = new TransactionRepository(db);
            var convertService = new ConvertService(transactionRepository);
            var transactionService = new TransactionService(convertService, transactionRepository);
            var result = await transactionService.TransferFunds(transactionRequest, "1");
            var accountsbalance = accounts[0].Balance;

            
            Assert.That(accounts[0].Balance, Is.EqualTo(98.5));
            Assert.That(accounts[1].Balance, Is.EqualTo(100));


        } 

        private DbContextOptions<AppDbContext> GetDbContextOptions()
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase("inmemorydb");
            return builder.Options;
        }
    }
}
