using BankingSystem.DB;
using BankingSystem.DB.Entities;
using BankingSystem.Features.Reports;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Tests
{
    public class ReportsServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            List<TransactionEntity> transactionsList = new List<TransactionEntity>()
            {
                new TransactionEntity() { Id = 1, Amount= 100, ConvertRate = 1, CreatedAt = DateTime.UtcNow, CurrencyFrom = Currency.GEL, CurrencyTo = Currency.GEL, FeeInGEL = 1.5m, TransactionType = TransactionType.Outer, RecipientAccount= "a", SenderAccount = "b" },
            };



            using var db = new AppDbContext(GetDbContextOptions());
            db.Database.EnsureCreated();
            db.Transactions.AddRange(transactionsList);
            await db.SaveChangesAsync();
            var reportsRepository = new ReportsRepository(db);
            var reportsService = new ReportsService(reportsRepository);
            var result = await reportsService.CalculateFees(DateTime.Now);
            Assert.That(result.avgFeeInGEL, Is.EqualTo(1.5m));
        }

        private DbContextOptions<AppDbContext> GetDbContextOptions()
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase("inmemorydb");
            return builder.Options;
        }
    }
}