using BankingSystem.DB;
using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.Auth;
using BankingSystem.Features.InternetBank.Operator.RegisterUser;
using BankingSystem.Features.InternetBank.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Features.InternetBank.Operator.AddUser
{
    public interface IRegisterUserRepository
    {
        public Task<IdentityResult> AddUserAsync(UserEntity entity, string password);
        public Task SaveChangesAsync();
        public Task<bool> UserExists(string personalNumber);
        
    }

    public class RegisterUserRepository : IRegisterUserRepository
    {
        private readonly AppDbContext _db;
        private readonly UserManager<UserEntity> _userManager;
        public RegisterUserRepository(AppDbContext db, UserManager<UserEntity> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IdentityResult> AddUserAsync(UserEntity entity, string password)
        {
            return await _userManager.CreateAsync(entity, password);
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
