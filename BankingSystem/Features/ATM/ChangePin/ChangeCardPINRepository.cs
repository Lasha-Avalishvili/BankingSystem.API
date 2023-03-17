using BankingSystem.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Features.ATM.ChangePin
{
    public interface IChangeCardPINRepository
    {
        Task<string> ChangePINAsync(ChangeCardPINRequest changePINRequest, string userId);
    }
    public class ChangeCardPINRepository : IChangeCardPINRepository
    {
        private readonly AppDbContext _db;

        public ChangeCardPINRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<string> ChangePINAsync(ChangeCardPINRequest changePINRequest, string userId)
        {
            var card = await _db.Cards
                .Include(c => c.Account)
                .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(c => c.CardNumber == changePINRequest.CardNumber);



            if (card != null && card.Account.UserId.ToString() == userId && changePINRequest.NewPIN != card.PIN)
            {
                Console.WriteLine(card.PIN);
                card.PIN = changePINRequest.NewPIN;
                _db.SaveChanges();
                return await Task.FromResult("Your Card Pin Successfully Changed");
            }

            return "Something Wrong Please Try Again";
        }
    }
}
