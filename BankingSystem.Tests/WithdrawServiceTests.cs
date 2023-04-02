//using BankingSystem.DB.Entities;
//using BankingSystem.DB;
//using BankingSystem.Features.InternetBank.User.Transactions;
//using Microsoft.EntityFrameworkCore;
//using BankingSystem.Features.ATM.Withdraw;
//using System.Runtime.Intrinsics.X86;

//namespace BankingSystem.Tests
//{
//    public class WithdrawServiceTests
//    {
//        [Test]
//        public async Task WithdrawServiceTest()
//        {
//            WithdrawRequest withdrawRequest = new WithdrawRequest()
//            {
//                Amount= 100, CardNumber="1111111111111111", Currency= Currency.GEL, PIN="1234"
//            };

//            List<AccountEntity> accounts = new List<AccountEntity>()
//            {
//                new AccountEntity() { Balance = 1000, Currency= 0, IBAN="1", Id=1, UserId= 1},
               
//            };

//            var card = new CardEntity { CardNumber = "1111111111111111", PIN = "1234", CVV="111", AccountId = 1, ExpirationDate = DateTime.UtcNow.AddDays(1) };


//            using var db = new AppDbContext(GetDbContextOptions());
//            db.Database.EnsureCreated();
//            db.Accounts.AddRange(accounts);
//            db.Cards.Add(card);
//            await db.SaveChangesAsync();
//            var WithdrawRepository = new WithdrawRepository(db);
//            var transacitonRepository = new TransactionRepository(db); // 
//            var convertService = new ConvertService(transacitonRepository); //i dont know what's the use of this
//            var withdrawService = new WithdrawService(convertService, WithdrawRepository);
//            var result = await withdrawService.WithdawFromAtm(withdrawRequest);
//            var accountsbalance = accounts[0].Balance;


//            Assert.That(accounts[0].Balance, Is.EqualTo(898));

//        }

//        private DbContextOptions<AppDbContext> GetDbContextOptions()
//        {
//            var builder = new DbContextOptionsBuilder<AppDbContext>();
//            builder.UseInMemoryDatabase("inmemorydb");
//            return builder.Options;
//        }
//    }
//}
