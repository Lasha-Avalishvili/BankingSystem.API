using BankingSystem.DB;
using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.Auth;
using BankingSystem.Features.InternetBank.Operator.RegisterUser;
using BankingSystem.Features.InternetBank.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Features.InternetBank.Operator.AddUser
{
    public interface IUserRepository
    {
        public Task AddUserAsync(UserEntity entity);
       // Task<UserEntity> RegisterUserAsync(RegisterUserRequest request);
        Task<string> LoginUserAsync(LoginUserRequest request);
        bool UserExists(string personalNumber);
        Task SaveChangesAsync();

    }

    public class RegisterUserRepository : IUserRepository
    {
        private readonly AppDbContext _db;
        private readonly TokenGenerator _tokenGenerator;
        public RegisterUserRepository(AppDbContext db, TokenGenerator tokenGenerator)
        {
            _db = db;
            _tokenGenerator = tokenGenerator;
        }

        public async Task AddUserAsync(UserEntity entity)
        {
            await _db.Users.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
        //public async Task<UserEntity> RegisterUserAsync(RegisterUserRequest request)
        //{
        //    var newUser = new UserEntity();
        //    newUser.FirstName = request.FirstName;
        //    newUser.LastName = request.LastName;
        //    newUser.Email = request.Email;
        //    newUser.RegisteredAt = DateTime.Now;
        //    newUser.DateOfBirth = request.DateOfBirth;
        //    newUser.PersonalNumber = request.PersonalNumber;
        //    newUser.Password = request.Password;

        //    await _db.Users.AddAsync(newUser);

        //    return newUser;
        //}

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public bool UserExists(string personalNumber)
        {
            return _db.Users.Any(u => u.PersonalNumber == personalNumber);
        }

        public async Task<string> LoginUserAsync(LoginUserRequest request)
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
