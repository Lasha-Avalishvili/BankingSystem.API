using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Features.InternetBank.Operator.AddUser
{
    public interface IRegisterUserRepository
    {
        public Task<IdentityResult> AddUserAsync(UserEntity entity, string password);
        public Task SaveChangesAsync();
        public Task<bool> UserExists(string personalNumber);
        public Task<IdentityResult> AddToRoleAsync(UserEntity user, string role);
        public Task<IList<string>> GetRoleAsync(UserEntity user);
        public Task<UserEntity> FindUser(string personalNumber);


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

        public async Task<IdentityResult> AddToRoleAsync(UserEntity user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<UserEntity> FindUser(string personalNumber)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.PersonalNumber == personalNumber);
            
            return user;
        }

        public async Task<IList<string>>GetRoleAsync(UserEntity user)
        {
            var userRole = await _userManager.GetRolesAsync(user);

            return userRole;
        }

        public async Task<bool> UserExists(string personalNumber)
        {
            return await _db.Users.AnyAsync(u => u.PersonalNumber == personalNumber);
        }
    }
}
