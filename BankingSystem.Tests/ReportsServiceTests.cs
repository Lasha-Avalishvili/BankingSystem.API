using BankingSystem.DB;
using BankingSystem.DB.Entities;
using BankingSystem.Features.Reports;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Tests
{
    public class ReportsServiceTests
    {
        [Test]
        public async Task AverageTransactionFeeTest()
        {
            List<TransactionEntity> transactionsList = new List<TransactionEntity>()
            {
                new TransactionEntity() { Id = 1, RecipientAccountId= null, SenderAccountId=1, Amount= 100, ConvertRate = 1, CreatedAt = DateTime.UtcNow.AddDays(-3), CurrencyFrom = 0, CurrencyTo = 0, FeeInGEL = 2m, TransactionType = TransactionType.ATM, RecipientAccount= "a", SenderAccount = "b" },
            };

            using var db = new AppDbContext(GetDbContextOptions());
            db.Database.EnsureCreated();
            db.Transactions.AddRange(transactionsList);
            await db.SaveChangesAsync();
            var reportsRepository = new ReportsRepository(db);
            var reportsService = new ReportsService(reportsRepository);
            var result = await reportsService.CalculateAverageTransactionFee(DateTime.Now.AddDays(-100));
            Assert.That(result.AverageTransactionFee.GEL, Is.EqualTo(2m));
        }

        private DbContextOptions<AppDbContext> GetDbContextOptions()
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase("inmemorydb");
            return builder.Options;
        }
    }
}