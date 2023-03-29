using BankingSystem.DB;
using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.Auth;
using BankingSystem.Features.InternetBank.Operator.RegisterUser;
using BankingSystem.Features.InternetBank.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Features.InternetBank.Operator.AddUser
{
    public interface IRegisterUserRepository
    {
        public Task AddUserAsync(UserEntity entity);
        public Task SaveChangesAsync();
        public Task<bool> UserExists(string personalNumber);
    }

    public class RegisterUserRepository : IRegisterUserRepository
    {
        private readonly AppDbContext _db;
        public RegisterUserRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddUserAsync(UserEntity entity)
        {
            await _db.Users.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<bool> UserExists(string personalNumber)
        {
            return await _db.Users.AnyAsync(u => u.PersonalNumber == personalNumber);
        }
    }
}
