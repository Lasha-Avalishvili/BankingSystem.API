using BankingSystem.DB.Entities;
using BankingSystem.DB;
using BankingSystem.Features.InternetBank.User.Transactions;
using Microsoft.EntityFrameworkCore;
using BankingSystem.Features.ATM.Withdraw;
using System.Runtime.Intrinsics.X86;

namespace BankingSystem.Tests
{
    public class WithdrawServiceTests
    {
        private AppDbContext _dbContext;

        [TestCase(100, Currency.GEL, 11898, true)]
        [TestCase(5000, Currency.USD, 12000, false)]
        [TestCase(11000, Currency.GEL, 12000, false)]




        [Test]
        public async Task WithdrawServiceTest(decimal amount, Currency currency, decimal expectedBalance, bool isPossible)
        {
            WithdrawRequest withdrawRequest = new WithdrawRequest()
            {
                Amount = amount,
                CardNumber = "1111111111111111",
                Currency = currency,
                PIN = "1234"
            };

            List<AccountEntity> accounts = new List<AccountEntity>()
            {
                new AccountEntity() { Balance = 12000, Currency= 0, IBAN="1", Id=1, UserId= 1},

            };

            var card = new CardEntity { CardNumber = "1111111111111111", PIN = "1234", CVV = "111", AccountId = 1, ExpirationDate = DateTime.UtcNow.AddDays(1) };

            var exchangeRates = new List<ExchangeRateEntity>()
            {
                new ExchangeRateEntity(){ id=1, QuoteCurrency="GEL", Rate= 1},
                new ExchangeRateEntity(){ id=2, QuoteCurrency="USD", Rate= 2.53m },
                new ExchangeRateEntity(){ id=3, QuoteCurrency="EUR", Rate= 2.77m }
            };

            _dbContext = new AppDbContext(GetDbContextOptions());
            _dbContext.Database.EnsureCreated();
            _dbContext.Accounts.AddRange(accounts);
            _dbContext.Cards.Add(card);
            _dbContext.ExchangeRates.AddRange(exchangeRates);
            await _dbContext.SaveChangesAsync();
            var WithdrawRepository = new WithdrawRepository(_dbContext);
            var transacitonRepository = new TransactionRepository(_dbContext);
            var convertService = new ConvertService(transacitonRepository);
            var withdrawService = new WithdrawService(convertService, WithdrawRepository);
            var result = await withdrawService.WithdawFromAtm(withdrawRequest);
            var accountsbalance = accounts[0].Balance;

            Assert.That(accounts[0].Balance, Is.EqualTo(expectedBalance));
            Assert.That(result.IsSuccessful, Is.EqualTo(isPossible));

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
