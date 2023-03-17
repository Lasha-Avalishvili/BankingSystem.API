using BankingSystem.DB;
using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.Auth;
using BankingSystem.Features.InternetBank.Operator.AuthUser;
using BankingSystem.Features.InternetBank.Operator.RegisterUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Features.InternetBank.Operator.AddUser
{
    public interface IUserRepository
    {
        Task<UserEntity> RegisterUserAsync(UserRegisterRequest request);
        Task<string> LoginUserAsync(UserLoginRequest request);
        bool UserExists(string personalNumber);
        Task SaveChangesAsync();

    }

    public class AuthUserRepository : IUserRepository
    {
        private readonly AppDbContext _db;
        private readonly TokenGenerator _tokenGenerator;
        public AuthUserRepository(AppDbContext db, TokenGenerator tokenGenerator)
        {
            _db = db;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<UserEntity> RegisterUserAsync(UserRegisterRequest request)
        {
            var newUser = new UserEntity();
            newUser.FirstName = request.FirstName;
            newUser.LastName = request.LastName;
            newUser.Email = request.Email;
            newUser.RegisteredAt = DateTime.Now;
            newUser.DateOfBirth = request.DateOfBirth;
            newUser.PersonalNumber = request.PersonalNumber;
            newUser.Password = request.Password;

            await _db.Users.AddAsync(newUser);

            return newUser;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public bool UserExists(string personalNumber)
        {
            return _db.Users.Any(u => u.PersonalNumber == personalNumber);
        }

        public async Task<string> LoginUserAsync(UserLoginRequest request)
        {
            var user = await _db.Users.Where(u => u.FirstName == request.FirstName).FirstOrDefaultAsync();

            if (user == null)
            {
                return ("User not found");
            }

            var userpass = await _db.Users.Where(u => u.Password == request.Password).FirstOrDefaultAsync();

            if (userpass == null)
            {
                return ("Invalid name or password");
            }

            return _tokenGenerator.GenerateForUser(user.Id.ToString());
        }
    }
}
