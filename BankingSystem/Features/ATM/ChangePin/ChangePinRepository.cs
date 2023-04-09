using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Features.ATM.ChangePin
{
    public interface IChangeCardPinRepository
    {
        public Task SaveChangesAsync();
        public Task<CardEntity> AuthorizeCardAsync(string cardNumber, string pin);
    }
    public class ChangePinRepository : IChangeCardPinRepository
    {
        private readonly AppDbContext _db;

        public ChangePinRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CardEntity> AuthorizeCardAsync(string cardNumber, string PIN)
        {
            var card = await _db.Cards.FirstOrDefaultAsync(c => c.CardNumber == cardNumber && c.PIN == PIN);
            return card;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
