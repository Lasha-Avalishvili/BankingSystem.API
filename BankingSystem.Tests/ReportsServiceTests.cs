using BankingSystem.DB;
using BankingSystem.DB.Entities;
using BankingSystem.Features.Reports;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Tests
{
    public class ReportsServiceTest
    {
        private AppDbContext _db;

        [TestCase(2, 4, 6, 4)]
        [TestCase(10 , 4, 6, 6.67)]
        [TestCase(0, 7, 11, 6)]

        [Test]
        public async Task AverageTransactionFeeTest(decimal fee1, decimal fee2, decimal fee3, decimal expectedResult)
        {
            List<TransactionEntity> transactionsList = new List<TransactionEntity>()
            {
                new TransactionEntity() { Id = 1, FeeInGEL = fee1,  Amount= 100, ConvertRate = 1, CreatedAt = DateTime.UtcNow.AddDays(-3), CurrencyFrom = 0, CurrencyTo = 0, TransactionType = TransactionType.ATM, RecipientAccount= "a", SenderAccount = "b" },
                new TransactionEntity() { Id = 2, FeeInGEL = fee2,  Amount= 100, ConvertRate = 1, CreatedAt = DateTime.UtcNow.AddDays(-3), CurrencyFrom = 0, CurrencyTo = 0, TransactionType = TransactionType.ATM, RecipientAccount= "a", SenderAccount = "b" },
                new TransactionEntity() { Id = 3, FeeInGEL = fee3,  Amount= 100, ConvertRate = 1, CreatedAt = DateTime.UtcNow.AddDays(-3), CurrencyFrom = 0, CurrencyTo = 0, TransactionType = TransactionType.ATM, RecipientAccount= "a", SenderAccount = "b" }
            };

            _db = new AppDbContext(GetDbContextOptions());
            _db.Database.EnsureCreated();
            _db.Transactions.AddRange(transactionsList);
            await _db.SaveChangesAsync();
            var reportsRepository = new ReportsRepository(_db);
            var reportsService = new ReportsService(reportsRepository);
            var result = await reportsService.CalculateAverageTransactionFee(DateTime.Now.AddDays(-100));
            Assert.That(result.AverageTransactionFee.GEL, Is.EqualTo(expectedResult));
        }

        [TearDown]
        public void Cleanup()
        {
            _db.Database.EnsureDeleted();
            _db.Dispose();
            _db = null;
        }
        private DbContextOptions<AppDbContext> GetDbContextOptions()
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase("inmemorydb");
            return builder.Options;
        }
    }
}