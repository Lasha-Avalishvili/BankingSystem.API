using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Features.ATM.AccountBlance
{
    public interface IGetBalanceRepository
    {
        public Task<CardEntity> GetCardAsync(GetBalanceRequest request);
    }
    public class GetBalanceRepository : IGetBalanceRepository
    {
        private readonly AppDbContext _db;
        public GetBalanceRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task <CardEntity> GetCardAsync(GetBalanceRequest request)
        {
            var card = await _db.Cards
                .Include(c => c.Account)
                .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(c => c.CardNumber == request.CardNumber && c.PIN == request.PIN);

            return card;
        }
    }
}
