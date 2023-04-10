using BankingSystem.DB.Entities;
using BankingSystem.DB;
using Microsoft.EntityFrameworkCore;
using BankingSystem.Features.InternetBank.User.Transactions;

namespace BankingSystem.Tests
{
    public class TransactionServiceTests
    {
        private AppDbContext _dbContext;

        [TestCase("1", "2", 100, 200, 0, 98.5, 100)]
        [TestCase("1", "2", 50, 200, 0, 149, 50)]
        [TestCase("1", "2", 200, 300, 0, 97.5, 200)]
        [TestCase("1", "2", 100, 100, 0, 100, 0)]

        [Test]
        public async Task TransferFundsExternalSameCurrencyTest(
            string senderIban,
            string recipientIban, 
            decimal transferAmount,
            decimal initialSenderBalance, 
            decimal initialRecipientBalance, 
            decimal expectedSenderBalance, 
            decimal expectedRecipientBalance)
        {
            TransactionRequest transactionRequest = new TransactionRequest()
            {
                SenderAccountIBAN = senderIban,
                RecipientAccountIBAN = recipientIban,
                Amount = transferAmount
            };

            List<AccountEntity> accounts = new List<AccountEntity>()
            {
                new AccountEntity() { Balance = initialSenderBalance, Currency= 0, IBAN=senderIban, Id=1, UserId= 1},
                new AccountEntity() { Balance = initialRecipientBalance, Currency= 0, IBAN=recipientIban, Id=2, UserId= 2},
            };

            _dbContext = new AppDbContext(GetDbContextOptions());
            _dbContext.Database.EnsureCreated();
            _dbContext.Accounts.AddRange(accounts);
            await _dbContext.SaveChangesAsync();
            var transactionRepository = new TransactionRepository(_dbContext);
            var convertService = new ConvertService(transactionRepository);
            var transactionService = new TransactionService(convertService, transactionRepository);
            var result = await transactionService.TransferFunds(transactionRequest, senderIban);

            Assert.That(accounts[0].Balance, Is.EqualTo(expectedSenderBalance));
            Assert.That(accounts[1].Balance, Is.EqualTo(expectedRecipientBalance));
        }


        [TestCase("1", "2", 100, 200, 0, 100, 100)]
        [TestCase("1", "2", 50, 200, 0, 150, 50)]
        [TestCase("1", "2", 200, 300, 0, 100, 200)]
        [TestCase("1", "2", 100, 0, 0, 0, 0)]

        [Test]
        public async Task TransferFundsInternalSameCurrencyTest(
            string senderIban,
            string recipientIban,
            decimal transferAmount,
            decimal initialSenderBalance,
            decimal initialRecipientBalance,
            decimal expectedSenderBalance,
            decimal expectedRecipientBalance)
        {
            TransactionRequest transactionRequest = new TransactionRequest()
            {
                SenderAccountIBAN = senderIban,
                RecipientAccountIBAN = recipientIban,
                Amount = transferAmount
            };

            List<AccountEntity> accounts = new List<AccountEntity>()
            {
                new AccountEntity() { Balance = initialSenderBalance, Currency= 0, IBAN=senderIban, Id=1, UserId= 1},
                new AccountEntity() { Balance = initialRecipientBalance, Currency= 0, IBAN=recipientIban, Id=2, UserId= 1},
            };

            _dbContext = new AppDbContext(GetDbContextOptions());
            _dbContext.Database.EnsureCreated();
            _dbContext.Accounts.AddRange(accounts);
            await _dbContext.SaveChangesAsync();
            var transactionRepository = new TransactionRepository(_dbContext);
            var convertService = new ConvertService(transactionRepository);
            var transactionService = new TransactionService(convertService, transactionRepository);
            var result = await transactionService.TransferFunds(transactionRequest, senderIban);

            Assert.That(accounts[0].Balance, Is.EqualTo(expectedSenderBalance));
            Assert.That(accounts[1].Balance, Is.EqualTo(expectedRecipientBalance));
        }

        [TearDown]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
            _dbContext = null;
        }

        private DbContextOptions<AppDbContext> GetDbContextOptions()
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase("inmemorydb");
            return builder.Options;
        }
    }
}

