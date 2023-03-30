using BankingSystem.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Features.ATM.AccountBlance
{
    public interface IGetAccountBalanceRepository
    {
        Task<GetAccountBalanceResponse> GetBalanceAsync(GetAccountBalanceRequest request);
    }
    public class GetAccountBalanceRepsoitory : IGetAccountBalanceRepository
    {
        private readonly AppDbContext _db;
        public GetAccountBalanceRepsoitory(AppDbContext db)
        {
            _db = db;
        }

        public async Task <GetAccountBalanceResponse> GetBalanceAsync(GetAccountBalanceRequest request)
        {
            var card = await _db.Cards
                .Include(c => c.Account)
                .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(c => c.CardNumber == request.CardNumber && c.PIN == request.PIN);

            if (card == null)
            {
                return new GetAccountBalanceResponse
                {
                    IsSuccessful = false,
                    ErrorMessage = "Invalid card number or PIN"
                };
            }

            var account = card.Account;

            return new GetAccountBalanceResponse
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Balance = account.Balance,
                Currency = account.Currency
            };
        }
    }
}
