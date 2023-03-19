using BankingSystem.DB;
using BankingSystem.DB.Entities;
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
    public interface IChangeCardPinRepository
    {
        Task<CardEntity> ChangePinAsync(ChangeCardPinRequest ChangePinRequest, string userId);
        Task SaveChangesAsync();
    }
    public class ChangeCardPinRepository : IChangeCardPinRepository
    {
        private readonly AppDbContext _db;

        public ChangeCardPinRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CardEntity> ChangePinAsync(ChangeCardPinRequest changePinRequest, string userId)
        {
            var card = await _db.Cards
                .Include(c => c.Account)
                .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(c => c.CardNumber == changePinRequest.CardNumber);

            
            return card;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
