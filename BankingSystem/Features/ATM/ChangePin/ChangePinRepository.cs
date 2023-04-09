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
