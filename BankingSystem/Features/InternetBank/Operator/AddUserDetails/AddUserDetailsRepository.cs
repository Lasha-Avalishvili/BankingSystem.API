using BankingSystem.DB;
using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.Operator.AddUser;
using IbanNet;

namespace BankingSystem.Features.InternetBank.Operator.AddAccountForUser
{
    public interface IAddUserDetailsRepository
    {
        public Task AddAccountAsync(AccountEntity entity);
        //Task<AccountEntity> AddAccountAsync(AddAccountRequest request);
        public Task AddCardAsync(CardEntity entity);

        // Task<CardEntity> AddCardAsync(AddCardRequest request);
        Task SaveChangesAsync();

        bool AccountExists(string iban);
    }
    public class AddUserDetailsRepository : IAddUserDetailsRepository
    {
        private readonly AppDbContext _db;
        public AddUserDetailsRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAccountAsync(AccountEntity entity)
        {
            await _db.Accounts.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
        //public async Task<AccountEntity> AddAccountAsync(AddAccountRequest request)
        //{

        //    var account = new AccountEntity();
        //    account.IBAN = request.IBAN;
        //    account.Currency = request.Currency;
        //    account.UserId = request.UserId;
        //    account.Balance = request.Amount;
        //    await _db.Accounts.AddAsync(account);

        //    return account;
        //}
        public async Task AddCardAsync(CardEntity entity)
        {
            await _db.Cards.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
        //public async Task<CardEntity> AddCardAsync(AddCardRequest request)
        //{
        //    var card = new CardEntity();
        //    card.AccountId = request.AccountId;
        //    card.FullName = request.FullName;
        //    card.CardNumber = request.CardNumber;
        //    card.ExpirationDate = request.ExpirationDate;
        //    card.CVV = request.CVV;
        //    card.PIN = request.PIN;
        //    await _db.Cards.AddAsync(card);

        //    return card;
        //}

        public bool AccountExists(string iban)
        {
            return _db.Accounts.Any(a => a.IBAN == iban);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
