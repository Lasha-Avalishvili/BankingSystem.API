using Azure;
using BankingSystem.DB;
using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace BankingSystem.Features.InternetBank.User.LoginUser
{
    public interface ILoginUserRepository
    {
        Task<LoginUserResponse> LoginUserAsync(LoginUserRequest request);
    }

    public class LoginUserRepository : ILoginUserRepository
    {
        private readonly AppDbContext _db;
        private readonly TokenGenerator _tokenGenerator;
        private readonly UserManager<UserEntity> _userManager;
        public LoginUserRepository(AppDbContext db, TokenGenerator tokenGenerator, UserManager<UserEntity> userManager)
        {
            _db = db;
            _tokenGenerator = tokenGenerator;
            _userManager = userManager;
        }

        public async Task<LoginUserResponse> LoginUserAsync(LoginUserRequest request)
        {
            var response = new LoginUserResponse();

            var user = await _userManager.FindByEmailAsync(request.PersonalNumber);
            if (user == null)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "User Not Found";
                response.JWT = null;
                return response;
            }

            var userpass = await _userManager.CheckPasswordAsync(user, request.Password);
            var roles = await _userManager.GetRolesAsync(user);

            if (userpass == false)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Invalid ID or password";
                response.JWT = null;
            }
            else
            {
                response.IsSuccessful = true;
                response.ErrorMessage = null;
                response.JWT = _tokenGenerator.Generate(roles, user.Id.ToString());
            }
            return response;
        }
    }
}
